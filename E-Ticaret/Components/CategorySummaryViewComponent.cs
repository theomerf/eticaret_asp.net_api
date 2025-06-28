using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Contracts;

namespace ETicaret.Components
{
    public class CategorySummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        private readonly IMemoryCache _cache;

        public CategorySummaryViewComponent(IServiceManager manager, IMemoryCache cache)
        {
            _manager = manager;
            _cache = cache;
        }

        public async Task<string> InvokeAsync()
        {
            string cacheKey = "allCategories";

            if (_cache.TryGetValue(cacheKey, out List<MainCategoryDto>? cachedCategories))
            {
                return cachedCategories?.Count.ToString()!;
            }

            var categories = await _manager.MainCategoryService.GetAllCategoriesAsync(false);
            return categories.Count().ToString();
        }
    }
}
