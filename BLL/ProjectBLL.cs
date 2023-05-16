using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using SD_340_W22SD_Final_Project_Group6.Models.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using System.Net;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class ProjectBLL
	{
		private IRepository<Project> _projectRepo;
		private IRepository<UserProject> _userProjectRepo;
		private IRepository<Ticket> _ticketRepo;
		private IRepository<TicketWatcher> _ticketWatcherRepo;
		private readonly UserManager<ApplicationUser> _users;

		public ProjectBLL(IRepository<Project> projectRepo, IRepository<UserProject> userProjectRepo, IRepository<Ticket> ticketRepo, IRepository<TicketWatcher> ticketWatcherRepo, UserManager<ApplicationUser> users) 
		{
			_projectRepo = projectRepo;
			_userProjectRepo = userProjectRepo;
			_ticketRepo = ticketRepo;
			_ticketWatcherRepo = ticketWatcherRepo;
			_users = users;
        }	

		public async Task<List<Project>> ListProjects(string? sortOrder, bool? sort, string? developerId, ApplicationUser currentUser)
		{
            List<Project> SortedProjs = new List<Project>();

            switch (sortOrder)
            {
                case "Priority":
                    if (sort == true)
                    {
                        // get all projects
                        SortedProjs = _projectRepo.GetAll().ToList();

                        // loop through all projects and assign their entity lists
                        foreach (Project project in SortedProjs)
                        {
                            // Fetch userProjects using ProjectDeveloperRepository
                            List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                            // Fetch Tickets using TicketRepository
                            List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id).OrderByDescending(t => t.TicketPriority).ToList();

                            // Assign the fetched entities to the project
                            project.AssignedTo = userProjects;
                            project.Tickets = tickets;
                        }
                    }
                    else
                    {
                        // get all projects
                        SortedProjs = _projectRepo.GetAll().ToList();

                        // loop through all projects and assign their entiy lists
                        foreach (Project project in SortedProjs)
                        {
                            // Fetch userProjects using ProjectDeveloperRepository
                            List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                            // Fetch Tickets using TicketRepository
                            List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id).OrderBy(t => t.TicketPriority).ToList();

                            // Assign the fetched entities to the project
                            project.AssignedTo = userProjects;
                            project.Tickets = tickets;
                        }
                    }

                break;

                case "RequiredHrs":
                    if (sort == true)
                    {
                        // get all projects
                        SortedProjs = _projectRepo.GetAll().ToList();

                        // loop through all projects and assign their entiy lists
                        foreach (Project project in SortedProjs)
                        {
                            // Fetch userProjects using ProjectDeveloperRepository
                            List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                            // Fetch Tickets using TicketRepository
                            List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id).OrderByDescending(t => t.RequiredHours).ToList();

                            // Assign the fetched entities to the project
                            project.AssignedTo = userProjects;
                            project.Tickets = tickets;
                        }
                    }
                    else
                    {
                        // get all projects
                        SortedProjs = _projectRepo.GetAll().ToList();

                        // loop through all projects and assign their entiy lists
                        foreach (Project project in SortedProjs)
                        {
                            // Fetch userProjects using ProjectDeveloperRepository
                            List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                            // Fetch Tickets using TicketRepository
                            List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id).OrderBy(t => t.RequiredHours).ToList();

                            // Assign the fetched entities to the project
                            project.AssignedTo = userProjects;
                            project.Tickets = tickets;
                        }
                    }

                break;

                case "Completed":

                    // get all projects
                    SortedProjs = _projectRepo.GetAll().ToList();

                    // loop through all projects and assign their entiy lists
                    foreach (Project project in SortedProjs)
                    {
                        // Fetch userProjects using ProjectDeveloperRepository
                        List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                        // Fetch Tickets using TicketRepository
                        List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id && t.Completed == true).ToList();

                        // Assign the fetched entities to the project
                        project.AssignedTo = userProjects;
                        project.Tickets = tickets;
                    }

                break;

                default:
                    if (developerId != null)
                    {
                        // get all projects
                        SortedProjs = _projectRepo.GetAll().ToList();

                        // loop through all projects and assign their entiy lists
                        foreach (Project project in SortedProjs)
                        {
                            // Fetch userProjects using ProjectDeveloperRepository
                            List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                            // Fetch Tickets using TicketRepository
                            List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id && t.Owner.Id.Equals(developerId)).ToList();

                            // Fetch Owner for each ticket using UserRepository
                            foreach (Ticket ticket in tickets)
                            {
                                // Fetch TicketWatchers using TicketWatcherRepository
                                ticket.TicketWatchers = _ticketWatcherRepo.GetAll().Where(tw => tw.TicketId == ticket.Id).ToList();
                            }

                            // Assign the fetched entities to the project
                            project.AssignedTo = userProjects;
                            project.Tickets = tickets;
                        }
                    }
                    else
                    {
                        // get all projects
                        SortedProjs = _projectRepo.GetAll().ToList();

                        // loop through all projects and assign their entiy lists
                        foreach (Project project in SortedProjs)
                        {
                            // Fetch userProjects using ProjectDeveloperRepository
                            List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == project.Id).ToList();

                            // Fetch Tickets using TicketRepository
                            List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == project.Id).ToList();

                            // Fetch Owner for each ticket using UserRepository
                            foreach (Ticket ticket in tickets)
                            {
                                // Fetch TicketWatchers using TicketWatcherRepository
                                ticket.TicketWatchers = _ticketWatcherRepo.GetAll().Where(tw => tw.TicketId == ticket.Id).ToList();
                            }

                            // Assign the fetched entities to the project
                            project.AssignedTo = userProjects;
                            project.Tickets = tickets;
                        }
                    }

                break;
            }

            //check if User is Project Manager or Developer
            IList<string> rolenames = await _users.GetRolesAsync(currentUser);

            var AssignedProjects = new List<Project>();

            // getting assigned project
            if (rolenames.Contains("Developer"))
            {
                AssignedProjects = SortedProjs.Where(p => p.AssignedTo.Select(projectUser => projectUser.UserId).Contains(currentUser.Id)).ToList();
            }
            else
            {
                AssignedProjects = SortedProjs;
            }

            if (AssignedProjects == null)
            {
                throw new NullReferenceException("No Projects available");
            }
            else
            {
                return AssignedProjects;  
            }
        }

		public List<Project> GetProjects()
		{
			return _projectRepo.GetAll().ToList();
		}

		public Project? GetProject(int? id) {

			if (id == null)
			{
				throw new NullReferenceException("Project Id cannot be null");
			}
			else
			{
				Project? project = _projectRepo.GetById(id);

				if (project == null)
				{
					throw new KeyNotFoundException();
				}
				else
				{
					return project;
				}
			}
		}

		public void CreateProject(CreateProjectVM vm)
		{
			if(vm == null)
			{
                throw new NullReferenceException("Project cannot be created");
            }
			else
			{
                vm.DeveloperIds.ForEach((developerId) =>
                {
					// get developer
                    ApplicationUser? developer = _users.Users.FirstOrDefault(u => u.Id == developerId);

					// create project
					Project project = new Project();
					project.ProjectName = vm.ProjectName;
					_projectRepo.Create(project);

					// create new project user
                    UserProject newUserProj = new UserProject();
                    newUserProj.ApplicationUser = developer;
                    newUserProj.UserId = developerId;
                    newUserProj.Project = project;
					_userProjectRepo.Create(newUserProj);

					// create relationship between project and project user
                    project.AssignedTo.Add(newUserProj);
                });
            }
		}

		public void EditProject(CreateProjectVM vm)
		{
			if (vm == null)
			{
                throw new NullReferenceException("Project cannot be created");
			}
			else
			{
				// get project
				Project project = _projectRepo.GetById(vm.Id);

				if (project == null)
				{
					throw new NullReferenceException("Project not found");
				}
				else
				{
					vm.DeveloperIds.ForEach((developerId) =>
					{
						// get developer
						ApplicationUser? developer = _users.Users.FirstOrDefault(u => u.Id == developerId);

						// update project user
						UserProject newUserProj = new UserProject();
						newUserProj.ApplicationUser = developer;
						newUserProj.UserId = developerId;
						newUserProj.Project = project;
						project.AssignedTo.Add(newUserProj);
					});

					// update project
					_projectRepo.Update(project);
				}
            }
		}

		public void DeleteProject(int id)
		{
			Project project = _projectRepo.GetById(id);

			if(project == null)
			{
                throw new NullReferenceException("Project not found");
            }
			else
			{
				// get project tickets
				List<Ticket> tickets = _ticketRepo.GetAll().Where(t => t.ProjectId == id).ToList();
				
				if(tickets != null)
				{
					// remove tickets
					tickets.ForEach(ticket => _ticketRepo.Delete(ticket));
				}

				// get project users
				List<UserProject> userProjects = _userProjectRepo.GetAll().Where(up => up.ProjectId == id).ToList();

				if(userProjects != null)
				{
					// remove user projects
					userProjects.ForEach(userProject => _userProjectRepo.Delete(userProject));
				}				

				// remove project
				_projectRepo.Delete(project);
			}
		}
    }
}
