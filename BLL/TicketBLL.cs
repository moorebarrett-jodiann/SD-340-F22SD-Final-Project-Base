using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using SD_340_W22SD_Final_Project_Group6.Models.ViewModel;
using System.Net.Sockets;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class TicketBLL
    {
        private IRepository<Project> _projectRepo;
        private IRepository<Ticket> _ticketRepo;
        private IRepository<TicketWatcher> _ticketWatcherRepo;
        private IRepository<Comment> _commentRepo;
        private readonly UserManager<ApplicationUser> _users;

        public TicketBLL(IRepository<Project> projectRepo, IRepository<Ticket> ticketRepo, IRepository<TicketWatcher> ticketWatcherRepo, IRepository<Comment> commentRepo, UserManager<ApplicationUser> users)
        {
            _projectRepo = projectRepo;
            _ticketRepo = ticketRepo;
            _ticketWatcherRepo = ticketWatcherRepo;
            _commentRepo = commentRepo;
            _users = users;
        }
        public List<Ticket> ListTickets()
        {
            // get all tickets
            List<Ticket> tickets = _ticketRepo.GetAll().ToList();

            // loop through all tickets and assign their entity lists
            foreach (Ticket ticket in tickets)
            {
                // Fetch ticketProject using ProjectRepository
                ticket.Project = _projectRepo.GetById(ticket.ProjectId);

                // Fetch TicketOwner using ApplicationUserRepository
                ticket.Owner = _users.Users.FirstOrDefault(u => u.Id == ticket.ApplicationUser);
            }

            if (tickets == null)
            {
                throw new NullReferenceException("No Tickets available");
            }
            else
            {
                return tickets;
            }
        }

        public Ticket GetTicketDetails(int id)
        {
            // get ticket
            Ticket ticket = _ticketRepo.GetById(id);

            // fetch ticket project
            ticket.Project = _projectRepo.GetById(ticket.ProjectId);

            // fetch ticket watchers
            List<TicketWatcher> ticketWatchers = _ticketWatcherRepo.GetAll().Where(tw => tw.TicketId == id).ToList();
            ticket.TicketWatchers = ticketWatchers;

            // fetch ticket watchers watcher
            foreach (TicketWatcher tWatcher in ticketWatchers)
            {
                tWatcher.Watcher = _users.Users.FirstOrDefault(u => u.Id == tWatcher.WatcherId);
            }

            // fetch ticket comments
            List<Comment> comments = _commentRepo.GetAll().Where(c => c.TicketId == id).ToList();
            ticket.Comments = comments;

            // fetch ticket owner
            ticket.Owner = _users.Users.FirstOrDefault(u => u.Id == ticket.ApplicationUser);

            // return ticket
            return ticket;
        }

        public List<Ticket> GetTickets()
        {
            return _ticketRepo.GetAll().ToList();
        }

        public Ticket? GetTicket(int? id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Ticket Id cannot be null");
            }
            else
            {
                Ticket? ticket = _ticketRepo.GetById(id);

                if (ticket == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    return ticket;
                }
            }
        }

        public void CreateTicket(CreateTicketVM vm)
        {
            if (vm == null)
            {
                throw new NullReferenceException("Ticket cannot be created");
            }
            else
            {
                // create ticket
                Ticket ticket = new Ticket();

                // set ticket project
                ticket.Project = _projectRepo.GetById(vm.ProjectId);

                // set ticket owner
                ticket.Owner = _users.Users.FirstOrDefault(u => u.Id == vm.ApplicationUser);

                // set other ticket properties
                ticket.Title = vm.Title;
                ticket.Body = vm.Body;
                ticket.RequiredHours = vm.RequiredHours;
                ticket.TicketPriority = vm.TicketPriority;

                // add ticket to db
                _ticketRepo.Create(ticket);
            }
        }

        public void UpdateTicketOwner(string ownerId, int ticketId)
        {
            if (ownerId == null || ticketId == null)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
                Ticket ticket = _ticketRepo.GetById(ticketId);

                ApplicationUser owner = _users.Users.FirstOrDefault(u => u.Id == ownerId);

                ticket.Owner = owner;
                _ticketRepo.Update(ticket);
            }
        }

        public void CommentTask(int ticketId, string comment, ApplicationUser user)
        {
            if (ticketId == null || comment == null)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
                Ticket ticket = _ticketRepo.GetById(ticketId);

                // create comment
                Comment newComment = new Comment();
                newComment.CreatedBy = user;
                newComment.Description = comment;
                newComment.Ticket = ticket;
                _commentRepo.Create(newComment);

                // create relationship between user and comment
                user.Comments.Add(newComment);

                // create relationship between ticker and comment
                ticket.Comments.Add(newComment);
            }
        }

        public void UpdateHours(int ticketId, int hours)
        {
            if (ticketId == null || hours == 0)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
				Ticket ticket = _ticketRepo.GetById(ticketId);

                // update hours
                ticket.RequiredHours = hours;
                _ticketRepo.Update(ticket);
			}
        }
        public void AddToWatchers(int ticketId, ApplicationUser user)
        {
            if (ticketId == null || user == null)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
				Ticket ticket = _ticketRepo.GetById(ticketId);

                // create ticket watcher
                TicketWatcher newTickWatch = new TicketWatcher();
				newTickWatch.Ticket = ticket;
				newTickWatch.Watcher = user;
                _ticketWatcherRepo.Create(newTickWatch);

                // create relationship between user and ticketwatcher
                user.TicketWatching.Add(newTickWatch);

                // create relationship between ticket and ticketwatcher
                ticket.TicketWatchers.Add(newTickWatch);
            }
		}

        public void RemoveFromWatchers(int ticketId, ApplicationUser user)
        {
            if (ticketId == null || user == null)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
                // get ticket
				Ticket ticket = _ticketRepo.GetById(ticketId);

                // get ticket watcher
                TicketWatcher? ticketWatcher = _ticketWatcherRepo.GetAll().Where(tw => tw.TicketId == ticketId && tw.WatcherId == user.Id).FirstOrDefault();

                // remove ticket watcher
                _ticketWatcherRepo.Delete(ticketWatcher);

				// remove ticket watcher relationship between ticket and watcher
				ticket.TicketWatchers.Remove(ticketWatcher);
                user.TicketWatching.Remove(ticketWatcher);
            }
		}

        public void MarkAsCompleted(int ticketId)
        {
            if (ticketId == null)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
				// get ticket
				Ticket ticket = _ticketRepo.GetById(ticketId);

                // set completed as true
                ticket.Completed = true;
                _ticketRepo.Update(ticket);
			}
		}
        
        public void UnmarkAsCompleted(int ticketId)
        {
            if (ticketId == null)
            {
                throw new NullReferenceException("Cannot update ticket");
            }
            else
            {
				// get ticket
				Ticket ticket = _ticketRepo.GetById(ticketId);

                // set completed as true
                ticket.Completed = false;
                _ticketRepo.Update(ticket);
			}
		}

		public void DeleteTicket(int id)
        {
            Ticket ticket = _ticketRepo.GetById(id);

            if (ticket == null)
            {
                throw new NullReferenceException("Ticket not found");
            }
            else
            {
                // get project
				Project currProj = _projectRepo.GetById(ticket.ProjectId);

                // remove relationship between project and ticket
                currProj.Tickets.Remove(ticket);

                // remove ticket
                _ticketRepo.Delete(ticket);
            }
        }
    }
}
