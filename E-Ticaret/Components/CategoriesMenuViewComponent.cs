using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public CategoriesMenuViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _manager.MainCategoryService.GetAllCategories(false);
            return View(categories);
        }
    }
}
