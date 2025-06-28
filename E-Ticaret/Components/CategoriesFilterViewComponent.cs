using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Contracts;

namespace ETicaret.Components
{
    public class CategoriesFilterViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        private readonly IMemoryCache _cache;

        public CategoriesFilterViewComponent(IServiceManager manager, IMemoryCache cache)
        {
            _manager = manager;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cacheKey = "allCategories";

            if (_cache.TryGetValue(cacheKey, out List<MainCategoryDto>? cachedCategories))
            {
                return View(cachedCategories);
            }

            var categories = await _manager.MainCategoryService.GetAllCategoriesAsync(false);
            return View(categories);
        }
    }
}
