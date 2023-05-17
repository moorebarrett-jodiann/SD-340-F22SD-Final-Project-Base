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
        private readonly UserManager<ApplicationUser> _users;
        private readonly TicketBLL _ticketBll;
        private readonly ProjectBLL _projectBll;        

        public TicketsController( IRepository<Ticket> ticketRepo, UserManager<ApplicationUser> users, IRepository<Project> projectRepo, IRepository<TicketWatcher> ticketWatcherRepo, IRepository<Comment> commentRepo ,IRepository<UserProject> userProjectRepo)
        {
            _users = users;
            _ticketBll = new TicketBLL(projectRepo, ticketRepo, ticketWatcherRepo, commentRepo,userProjectRepo ,users);
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
                 CreateTicketVM vm = _ticketBll.BuildTicketVM(id);
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
            try
			{
                CreateTicketVM vm = _ticketBll.BuildTicketVM(id);
                 return View(vm);
            }
			catch (Exception ex)
			{
				return NotFound();
			}
         
        }        

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit([Bind("Id,Title,Body,RequiredHours,TicketPriority,ProjectId,ApplicationUser,Developers")] CreateTicketVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
					_ticketBll.EditTicket(vm);
					return RedirectToAction(nameof(Edit), new { id = vm.Id });
				} 
                else
                {
                    return View(vm);
                }
            } 
            catch (Exception ex)
            {
                return NotFound();
            }
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

