using Microsoft.AspNetCore.Mvc.Rendering;

namespace SD_340_W22SD_Final_Project_Group6.Models.ViewModel
{
	public class CreateProjectVM
	{
		public int? Id { get; set; }
		public string ProjectName { get; set; }
		public List<SelectListItem> Developers { get; set; } = new List<SelectListItem>();
		public ICollection<UserProject>? AssignedTo { get; set; } = new HashSet<UserProject>();
		public List<string> DeveloperIds { get; set; } = new List<string>();

		public void PopulateList(IEnumerable<ApplicationUser> developers)
		{
			Developers.Add(new SelectListItem { Text = "-- Select Developers --", Value = "" });

			foreach (ApplicationUser u in developers)
			{
				Developers.Add(new SelectListItem($"{u.UserName}", u.Id));
			}
		}
		public CreateProjectVM(IEnumerable<ApplicationUser> developers)
		{
			PopulateList(developers);
		}

		public CreateProjectVM() { }
	}
}
