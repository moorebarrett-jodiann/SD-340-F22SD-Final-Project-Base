using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.Data
{
    public class TicketWatcherRepository : IRepository<TicketWatcher>
    {
        private ApplicationDbContext _context;
        public TicketWatcherRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(TicketWatcher entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TicketWatcher entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public ICollection<TicketWatcher> GetAll()
        {
            return _context.TicketWatchers.ToList();
        }

        public TicketWatcher? GetById(int? id)
        {
            return _context.TicketWatchers.First(tw => tw.Id == id);
        }

        public void Update(TicketWatcher entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
