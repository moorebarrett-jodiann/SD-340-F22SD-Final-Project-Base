using Microsoft.AspNetCore.Identity;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class TicketBLL
    {
        private IRepository<Project> _projectRepo;
        private IRepository<Ticket> _ticketRepo;
        private readonly UserManager<ApplicationUser> _users;

        public TicketBLL(IRepository<Project> projectRepo, IRepository<Ticket> ticketRepo, UserManager<ApplicationUser> users)
        {
            _projectRepo = projectRepo;
            _ticketRepo = ticketRepo;
            _users = users;
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

        public void DeleteTicket(int id)
        {
            Ticket ticket = _ticketRepo.GetById(id);

            if (ticket == null)
            {
                throw new NullReferenceException("Ticket not found");
            }
            else
            {
                // remove ticket
                _ticketRepo.Delete(ticket);
            }
        }
    }
}
