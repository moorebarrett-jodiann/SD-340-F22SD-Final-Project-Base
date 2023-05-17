using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.BLL;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using SD_340_W22SD_Final_Project_Group6.Models.ViewModel;

namespace SD_340_W22SD_Final_Project_Group6.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly ApplicationUserBLL _applicationUserBLL;

        public AdminController(IRepository<ApplicationUser> applicationUserRepository, UserManager<ApplicationUser> users)
        {
            _users = users;
            _applicationUserBLL = new ApplicationUserBLL(applicationUserRepository, users);
        }
        public async Task<IActionResult> Index()
        {
            ProjectManagersAndDevelopersViewModels vm = new ProjectManagersAndDevelopersViewModels();

            List<ApplicationUser> pmUsers = (List<ApplicationUser>)await _users.GetUsersInRoleAsync("ProjectManager");
            List<ApplicationUser> devUsers = (List<ApplicationUser>)await _users.GetUsersInRoleAsync("Developer");
            List<ApplicationUser> allUsers = _applicationUserBLL.GetUsers().ToList();

            vm.pms = pmUsers;
            vm.devs = devUsers;
            vm.allUsers = allUsers;
            return View(vm);
        }

        public async Task<IActionResult> ReassignRole()
        {
            List<ApplicationUser> users = _applicationUserBLL.GetUsers().ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReassignRole(string role, string userId)
        {
            await _applicationUserBLL.AssignUserRole(role, userId);
			return RedirectToAction("Index", "Admin", new { area = "" });
		}
    }
}

