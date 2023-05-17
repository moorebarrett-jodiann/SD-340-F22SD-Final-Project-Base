using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.BLL;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using SD_340_W22SD_Final_Project_Group6.Models.ViewModel;

namespace SD_340_W22SD_Final_Project_Group6.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _users;
        private readonly TicketBLL _ticketBll;
        private readonly ProjectBLL _projectBll;        

        public TicketsController(ApplicationDbContext context, IRepository<Ticket> ticketRepo, UserManager<ApplicationUser> users, IRepository<Project> projectRepo, IRepository<TicketWatcher> ticketWatcherRepo, IRepository<Comment> commentRepo, IRepository<UserProject> userProjectRepo)
        {
            _context = context;
            _users = users;
            _ticketBll = new TicketBLL(projectRepo, ticketRepo, ticketWatcherRepo, commentRepo, users);
            _projectBll = new ProjectBLL(projectRepo, userProjectRepo, ticketRepo, ticketWatcherRepo, users);
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Ticket> tickets = _ticketBll.ListTickets();
                return View(tickets);
            } 
            catch(Exception ex)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
             
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id == null || _ticketBll.GetTicket(id) == null)
                {
                    return NotFound();
                }

                Ticket ticket = _ticketBll.GetTicketDetails(id);

				List<UserProject> projectUsers = _projectBll.GetProjectUsers(ticket.ProjectId).ToList();

				CreateTicketVM vm = new CreateTicketVM
				{
                    Id = id,
                    Title = ticket.Title,
                    Body = ticket.Body,
                    RequiredHours = ticket.RequiredHours,
                    ApplicationUser = ticket.ApplicationUser,
                    TicketWatchers = ticket.TicketWatchers.ToList(),
                    Comments = ticket.Comments.ToList(),
                    Owner = ticket.Owner,
                    TicketPriority = ticket.TicketPriority,
                    Completed = ticket.Completed,
                    Project = ticket.Project,
					Developers = projectUsers.Select(d => new SelectListItem
					{
						Value = d.ApplicationUser.Id,
						Text = d.ApplicationUser.UserName
					}).ToList()
				};

				return View(vm);
            }
			catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Tickets/Create
        [Authorize(Roles = "ProjectManager")]
        public IActionResult Create(int projId)
        {
			try
			{
                Project project = _projectBll.GetProject(projId);

				List<UserProject> projectUsers = _projectBll.GetProjectUsers(projId).ToList();

				CreateTicketVM vm = new CreateTicketVM
				{
					Developers = projectUsers.Select(d => new SelectListItem
					{
						Value = d.ApplicationUser.Id,
						Text = d.ApplicationUser.UserName
					}).ToList(),
                    Project = project
				};

				return View(vm);
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,RequiredHours,TicketPriority,ProjectId,ApplicationUser,Developers")] CreateTicketVM vm)
        {
            try
            {
				_ticketBll.CreateTicket(vm);
				return RedirectToAction("Index", "Tickets", new { area = "" });
			}
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(t => t.Owner).FirstAsync(t => t.Id == id);
      
            if (ticket == null)
            {
                return NotFound();
            }

            List<ApplicationUser> results = _context.Users.Where(u => u != ticket.Owner).ToList();

            List<SelectListItem> currUsers = new List<SelectListItem>();
            results.ForEach(r =>
            {
                currUsers.Add(new SelectListItem(r.UserName, r.Id.ToString()));
            });
            ViewBag.Users = currUsers;

            return View(ticket);
        }        

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit(int id,string userId, [Bind("Id,Title,Body,RequiredHours")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser currUser = _context.Users.FirstOrDefault(u => u.Id == userId);
                    ticket.Owner = currUser;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new {id = ticket.Id});
            }
            return View(ticket);
        }

		[Authorize(Roles = "ProjectManager")]
		public async Task<IActionResult> RemoveAssignedUser(string id, int ticketId)
		{
            try
            {
                _ticketBll.UpdateTicketOwner(id, ticketId);
			    return RedirectToAction("Edit", new { id = ticketId });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
		}

		[HttpPost]
        public async Task<IActionResult> CommentTask(int TaskId, string? TaskText)
        {
            try
            {
				string userName = User.Identity.Name;
				ApplicationUser user = _users.Users.FirstOrDefault(u => u.UserName == userName);

				_ticketBll.CommentTask(TaskId, TaskText, user);
				return RedirectToAction("Index");
			}
            catch (Exception ex)
            {
				return RedirectToAction("Error", "Home");
			}
        }

        public async Task<IActionResult> UpdateHrs(int id, int hrs)
        {
            try
            {
                _ticketBll.UpdateHours(id, hrs);
                return RedirectToAction("Details", new { id });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> AddToWatchers(int id)
        {
            try
            {
				string userName = User.Identity.Name;
				ApplicationUser user = _users.Users.FirstOrDefault(u => u.UserName == userName);

                _ticketBll.AddToWatchers(id, user);
				return RedirectToAction("Details", new { id });

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> UnWatch(int id)
        {
            try
            {
				string userName = User.Identity.Name;
				ApplicationUser user = _users.Users.FirstOrDefault(u => u.UserName == userName);

				_ticketBll.RemoveFromWatchers(id, user);
				return RedirectToAction("Details", new { id });

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            try
            {
                _ticketBll.MarkAsCompleted(id);
                return RedirectToAction("Details", new { id });

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> UnMarkAsCompleted(int id)
        {
			try
			{
				_ticketBll.UnmarkAsCompleted(id);
				return RedirectToAction("Details", new { id });

			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}


        // GET: Tickets/Delete/5
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
			try
			{
				return View(_ticketBll.GetTicket(id));
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_ticketBll.GetTickets == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
                }

                _ticketBll.DeleteTicket(id);
				return RedirectToAction("Index", "Projects");
            }
			catch (Exception ex)
            {
                return NotFound();
            }
		}

		private bool TicketExists(int id)
		{
			if (_ticketBll.GetTicket(id) != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

