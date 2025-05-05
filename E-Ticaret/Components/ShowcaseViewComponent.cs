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

        public IViewComponentResult Invoke(string page = "default")
        {
            if (page == "default")
            {
                var products = _manager.ProductService.GetShowcaseProductsWithRatings(false).ToList();

                return View(products); 
            }
            else
            {
                var products = _manager.ProductService.GetShowcaseProducts(false).ToList();
                return View("List", products); 
            }
        }

    }
}
