using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.Data
{
    public class UserProjectRepository : IRepository<UserProject>
    {
        private ApplicationDbContext _context;
        public UserProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(UserProject entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(UserProject entity)
        {
            _context.Remove(entity);
            _context.SaveChanges(); 
        }

        public ICollection<UserProject> GetAll()
        {
            return _context.UserProjects.ToList(); 
        }

        public UserProject? GetById(int? id)
        {
            return _context.UserProjects.First(up => up.Id == id);
        }

        public void Update(UserProject entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
