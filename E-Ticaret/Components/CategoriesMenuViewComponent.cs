using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services.Contracts;

namespace ETicaret.Components
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        private readonly IMemoryCache _cache;

        public CategoriesMenuViewComponent(IServiceManager manager, IMemoryCache cache)
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

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(cacheKey, categories, cacheOptions);

            return View(categories);
        }
    }
}
