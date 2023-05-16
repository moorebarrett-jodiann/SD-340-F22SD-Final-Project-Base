using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
	public class UserProjectBLL
	{
		private IRepository<UserProject> _userProjectRepo;
		private IRepository<Project> _projectRepo;
		private readonly UserManager<ApplicationUser> _users;

		public UserProjectBLL(IRepository<UserProject> userProjectRepo, IRepository<Project> projectRepo, UserManager<ApplicationUser> users)
		{
			_userProjectRepo = userProjectRepo;
			_projectRepo = projectRepo;
			_users = users;
		}

		public int RemoveAssignedUser(string userId, int projectId)
		{
			if (userId == null)
			{
				throw new NullReferenceException("User Id cannot be null");
			}
			else
			{
				UserProject currUserProj = _userProjectRepo.GetAll().Where(up => up.ProjectId == projectId && up.UserId == userId).First();

				if (currUserProj == null)
				{
					throw new NullReferenceException("No projects found for current user");
				}
				else
				{
					_userProjectRepo.Delete(currUserProj);
					return projectId;
				}
			}
		}

		public List<UserProject> GetAllProjectUsers(int? projectId)
		{
			if (projectId == null)
			{
				throw new NullReferenceException("Project Id cannot be null");
			}
			else
			{
				Project? project = _projectRepo.GetById(projectId);

				// filter all project users assigned to this project
				return _userProjectRepo.GetAll().Where(up => up.ProjectId == projectId).ToList();
			}
		}
    }
}
