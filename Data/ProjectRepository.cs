using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.Data
{
    public class ProjectRepository : IRepository<Project>
    {
        private ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(Project entity)
        {
            _context.Projects.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Project entity)
        {
            _context.Projects.Remove(entity);
            _context.SaveChanges();
        }

        public ICollection<Project> GetAll()
        {
            return _context.Projects.ToList();
        }

        public Project? GetById(int? id)
        {
            return _context.Projects.Find(id);
        }

        public void Update(Project entity)
        {
            _context.Projects.Update(entity);
            _context.SaveChanges();
        }
    }
}
