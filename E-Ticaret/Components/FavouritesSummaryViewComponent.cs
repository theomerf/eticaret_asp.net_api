using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

namespace ETicaret.Components
{
    public class FavouritesSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public FavouritesSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? userId = (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);
            int favouritesCount = 0;

            if (userId != null)
            {
                var user = await _manager.AuthService.GetOneUser(User.Identity?.Name ?? string.Empty);
                favouritesCount = user?.FavouriteProductsId?.Count() ?? 0;
            }

            return View(favouritesCount);
        }
    }
}
