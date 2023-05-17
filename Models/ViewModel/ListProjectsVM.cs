using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace SD_340_W22SD_Final_Project_Group6.Models.ViewModel
{
    public class ListProjectsVM
    {
        public IPagedList<Project> Projects { get; set; }

        public List<SelectListItem> Developers { get; set; } = new List<SelectListItem>();

        public void PopulateList(IEnumerable<ApplicationUser> developers)
        {
            Developers.Add(new SelectListItem { Text = "-- Select Developers --", Value = "" });

            foreach (ApplicationUser u in developers)
            {
                Developers.Add(new SelectListItem($"{u.UserName}", u.Id));
            }
        }
      
        public ListProjectsVM() { }
        public ListProjectsVM(IPagedList<Project> projects,IEnumerable<ApplicationUser> developers)
        {
            PopulateList(developers);
            Projects = projects;

        }
    }
}
