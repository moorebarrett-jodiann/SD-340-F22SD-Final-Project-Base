using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.BLL;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using SD_340_W22SD_Final_Project_Group6.Models.ViewModel;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Mvc;


namespace SD_340_W22SD_Final_Project_Group6.Controllers
{
    [Authorize(Roles = "ProjectManager, Developer")]
    public class ProjectsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _users;
        private readonly ProjectBLL _projectBll;
        private readonly UserProjectBLL _userProjectBll;

        public ProjectsController(IRepository<Project> projectRepo, IRepository<UserProject> userProjectRepo, IRepository<Ticket> ticketRepo, IRepository<TicketWatcher> ticketWatcherRepo, UserManager<ApplicationUser> users, ApplicationDbContext context)
        {
            _projectBll = new ProjectBLL(projectRepo, userProjectRepo, ticketRepo, ticketWatcherRepo, users);
            _userProjectBll = new UserProjectBLL(userProjectRepo, projectRepo, users);
            _users = users;
			//_context = context;
		}

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index(string? sortOrder, int? page, bool? sort, string? userId)
        {
            //List<Project> SortedProjs = new List<Project>();
            
            //switch (sortOrder)
            //{
            //    case "Priority":
            //        if (sort == true)
            //        {
            //            SortedProjs =
            //            await _context.Projects
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets.OrderByDescending(t => t.TicketPriority))
            //            .ThenInclude(t => t.Owner)
            //            .ToListAsync();
            //        }
            //        else
            //        {
            //            SortedProjs =
            //            await _context.Projects
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets.OrderBy(t => t.TicketPriority))
            //            .ThenInclude(t => t.Owner)
            //            .ToListAsync();
            //        }

            //        break;
            //    case "RequiredHrs":
            //        if (sort == true)
            //        {
            //            SortedProjs =
            //            await _context.Projects
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets.OrderByDescending(t => t.RequiredHours))
            //            .ThenInclude(t => t.Owner)
            //            .ToListAsync();
            //        }
            //        else
            //        {
            //            SortedProjs =
            //            await _context.Projects
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets.OrderBy(t => t.RequiredHours))
            //            .ThenInclude(t => t.Owner)
            //            .ToListAsync();
            //        }

            //        break;
            //    case "Completed":
            //        SortedProjs =
            //            await _context.Projects
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets.Where(t => t.Completed == true))
            //            .ThenInclude(t => t.Owner)
            //            .ToListAsync();
            //        break;
            //    default:
            //        if (userId != null)
            //        {
            //            SortedProjs =
            //            await _context.Projects
            //            .OrderBy(p => p.ProjectName)
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets.Where(t => t.Owner.Id.Equals(userId)))
            //            .ThenInclude(t => t.Owner)
            //            .Include(p => p.Tickets).ThenInclude(t => t.TicketWatchers).ThenInclude(tw => tw.Watcher)
            //            .ToListAsync();
            //        }
            //        else
            //        {
            //            SortedProjs =
            //            await _context.Projects
            //            .OrderBy(p => p.ProjectName)
            //            .Include(p => p.CreatedBy)
            //            .Include(p => p.AssignedTo)
            //            .ThenInclude(at => at.ApplicationUser)
            //            .Include(p => p.Tickets)
            //            .ThenInclude(t => t.Owner)
            //            .Include(p => p.Tickets).ThenInclude(t => t.TicketWatchers).ThenInclude(tw => tw.Watcher)
            //            .ToListAsync();
            //        }

            //        break;
            //}
            ////check if User is PM or Develoer
            //var LogedUserName = User.Identity.Name;  // logined user name
            //var user = _context.Users.FirstOrDefault(u => u.UserName == LogedUserName);
            //var rolenames = await _users.GetRolesAsync(user);
            //var AssinedProject = new List<Project>();
            //// geting assined project
            //if (rolenames.Contains("Developer"))
            //{
            //    AssinedProject = SortedProjs.Where(p => p.AssignedTo.Select(projectUser => projectUser.UserId).Contains(user.Id)).ToList();
            //}
            //else
            //{
            //    AssinedProject = SortedProjs;
            //}
            try
            {
				List<ApplicationUser> allUsers = (List<ApplicationUser>)await _users.GetUsersInRoleAsync("Developer");

				List<SelectListItem> users = new List<SelectListItem>();
				allUsers.ForEach(au =>
				{
					users.Add(new SelectListItem(au.UserName, au.Id.ToString()));
				});
				ViewBag.Users = users;

				ApplicationUser currentUser = await _users.GetUserAsync(User); // get user's all data

                List<Project> projects = await _projectBll.ListProjects(sortOrder, sort, userId, currentUser);

                X.PagedList.IPagedList<Project> projList = projects.ToPagedList(page ?? 1, 3);

                return View(projList);
            }
            catch (Exception ex)
            {
                return NotFound();
            }            
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			try
			{
				return View(_projectBll.GetProject(id));
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
        }

        public async Task<IActionResult> RemoveAssignedUser(string id, int projId)
        {
			try
			{
				int projectId = _userProjectBll.RemoveAssignedUser(id, projId);
				return RedirectToAction("Edit", new { id = projId });
			}
			catch (Exception ex)
			{
                return NotFound();
            }
        }

        // GET: Projects/Create
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> CreateAsync()
        {
            try
            {
                List<ApplicationUser> developers = (List<ApplicationUser>)await _users.GetUsersInRoleAsync("Developer");
			    CreateProjectVM vm = new CreateProjectVM
			    {
				    Developers = developers.Select(d => new SelectListItem
				    {
					    Value = d.Id,
					    Text = d.UserName
				    }).ToList()
			    };

			    return View(vm);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
		}

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,Developers,DeveloperIds,AssignedTo")] CreateProjectVM vm)
        {
            try
            {                
                _projectBll.CreateProject(vm);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex) 
            {
                return NotFound();
            }
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                Project? project = _projectBll.GetProject(id);

                if (project == null)
                {
                    return NotFound();
                }

                List<ApplicationUser> results = _users.Users.ToList();

                CreateProjectVM vm = new CreateProjectVM
                {
                    Id = id,
                    ProjectName = project.ProjectName,
                    AssignedTo = _userProjectBll.GetAllProjectUsers(id),
                    Developers = results.Select(d => new SelectListItem
                    {
                        Value = d.Id,
                        Text = d.UserName
                    }).ToList()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                return NotFound();
            }            
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit([Bind("Id,ProjectName,Developers,DeveloperIds,AssignedTo")] CreateProjectVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _projectBll.EditProject(vm);
                    return RedirectToAction(nameof(Edit), new { id = vm.Id });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                return View(_projectBll.GetProject(id));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_projectBll.GetProjects == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
                }

                _projectBll.DeleteProject(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
