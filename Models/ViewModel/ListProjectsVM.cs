using Microsoft.AspNetCore.Mvc.Rendering;

namespace SD_340_W22SD_Final_Project_Group6.Models.ViewModel
{
    public class ListProjectsVM
    {
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<SelectListItem> Developers { get; set; } = new List<SelectListItem>();
        public List<string> DeveloperIds { get; set; } = new List<string>();
		public ICollection<UserProject> AssignedTo { get; set; } = new HashSet<UserProject>();

		public void PopulateList(IEnumerable<ApplicationUser> developers, IEnumerable<Project> projects)
        {
            Developers.Add(new SelectListItem { Text = "-- Select Developers --", Value = "" });

            foreach (ApplicationUser u in developers)
            {
                Developers.Add(new SelectListItem($"{u.UserName}", u.Id));
            }
            
            foreach (Project p in projects)
            {
                Projects.Add(p);
            }
        }
        public ListProjectsVM(IEnumerable<ApplicationUser> developers, IEnumerable<Project> projects) 
        {
            PopulateList(developers, projects);
        }
        public ListProjectsVM() { }
    }
}
