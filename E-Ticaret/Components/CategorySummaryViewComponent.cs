using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class CategorySummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public CategorySummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public string Invoke()
        {
            var categories = _manager.MainCategoryService.GetAllCategories(false);
            return categories.Count().ToString();
        }
    }
}
