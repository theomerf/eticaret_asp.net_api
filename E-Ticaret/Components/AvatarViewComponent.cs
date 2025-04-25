using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public AvatarViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<string> InvokeAsync()
        {
            var user = await _manager.AuthService.GetOneUser(User.Identity.Name);
            if (user != null)
            {
                return user.AvatarUrl;
            }
            return "avatars/default.png";
        }
    }
}
