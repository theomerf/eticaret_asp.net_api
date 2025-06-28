using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class UserCountsViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        private readonly UserManager<Account> _userManager;

        public UserCountsViewComponent(IServiceManager manager, UserManager<Account> userManager)
        {
            _manager = manager;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string mode)
        {
            if (mode == "users")
            {
                var users = await _manager.AuthService.GetAllUsersAsync();
                return Content(users.Count().ToString());
            }
            else if (mode == "admin")
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                return Content(adminUsers.Count.ToString());
            }
            else if (mode == "active")
            {
                var users = await _manager.AuthService.GetAllUsersAsync();
                var activeCount = users.Count(u => u.EmailConfirmed);
                return Content(activeCount.ToString());
            }
            else if (mode == "passive")
            {
                var users = await _manager.AuthService.GetAllUsersAsync();
                var passiveCount = users.Count(u => !u.EmailConfirmed);
                return Content(passiveCount.ToString());
            }

            return Content("Bilinmeyen mod");
        }
    }
}
