using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class UserSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public UserSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<string> InvokeAsync()
        {
            var users = await _manager.AuthService.GetAllUsersAsync();
            return users.Count().ToString();
        }
    }
}
