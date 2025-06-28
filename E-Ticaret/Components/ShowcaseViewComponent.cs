using ETicaret.Infrastructure.CookieHelpler;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class ShowcaseViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ShowcaseViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string page = "default")
        {
            if (page == "default")
            {
                var products = await _manager.ProductService.GetShowcaseProductsWithRatingsAsync(false);
                var favIds = CookieHelper.GetFavouriteProductIds(Request);
                ViewBag.FavouriteIds = CookieHelper.GetFavouriteProductIds(Request);

                return View(products); 
            }
            else
            {
                var products = await _manager.ProductService.GetShowcaseProductsAsync(false);
                return View("List", products); 
            }
        }

    }
}
