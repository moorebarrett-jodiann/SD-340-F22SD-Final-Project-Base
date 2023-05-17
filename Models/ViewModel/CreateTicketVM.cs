using Microsoft.AspNetCore.Mvc.Rendering;
using static SD_340_W22SD_Final_Project_Group6.Models.Ticket;

namespace SD_340_W22SD_Final_Project_Group6.Models.ViewModel
{
	public class CreateTicketVM
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public int RequiredHours { get; set; }
		public ApplicationUser? Owner { get; set; }
		public string ApplicationUser { get; set; }
		public List<SelectListItem> Developers { get; set; } = new List<SelectListItem>();
		public List<TicketWatcher>? TicketWatchers { get; set; } = new List<TicketWatcher>();
		public List<Comment> Comments { get; set; } = new List<Comment>();
		public Project? Project { get; set; }
		public int ProjectId { get; set; }
		public Priority? TicketPriority { get; set; }
		public bool? Completed { get; set; } = false;

		public void PopulateList(IEnumerable<ApplicationUser> developers)
		{
			Developers.Add(new SelectListItem { Text = "-- Select Developers --", Value = "" });

			foreach (ApplicationUser u in developers)
			{
				Developers.Add(new SelectListItem($"{u.UserName}", u.Id));
			}
		}
		public CreateTicketVM(IEnumerable<ApplicationUser> developers, Project project)
		{
			PopulateList(developers);
			Project = project;
		}

		public CreateTicketVM() { }
	}
}
