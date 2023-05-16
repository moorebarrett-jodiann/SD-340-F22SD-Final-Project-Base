using Microsoft.AspNetCore.Identity;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using System.Data;
using System.Web.Mvc;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class ApplicationUserBLL
    {
		private readonly UserManager<ApplicationUser> _users;
		private IRepository<ApplicationUser> _applicationUserRepo;

        public ApplicationUserBLL(IRepository<ApplicationUser> applicationUserRepo, UserManager<ApplicationUser> users)
        {
            _applicationUserRepo = applicationUserRepo;
			_users = users;
		}

        public ICollection<ApplicationUser> GetUsers()
        {
            return _applicationUserRepo.GetAll();
        }

        public async Task AssignUserRole(string role, string userId)
        {
			ApplicationUser user = _users.Users.First(u => u.Id == userId);

			ICollection<string> userRoles = await _users.GetRolesAsync(user);
			if (userRoles.Count == 0)
			{
				await _users.AddToRoleAsync(user, role);				
			}
			else
			{
				await _users.RemoveFromRoleAsync(user, userRoles.First());
				await _users.AddToRoleAsync(user, role);
			}
		}
    }
}
