using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Contracts;

namespace ETicaret.Components
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        private readonly IMemoryCache _cache;

        public AvatarViewComponent(IServiceManager manager, IMemoryCache cache)
        {
            _manager = manager;
            _cache = cache;
        }

        public async Task<string> InvokeAsync()
        {
            string cacheKey = "user";

            if (_cache.TryGetValue(cacheKey, out UserDto? cachedUser))
            {
                return cachedUser?.AvatarUrl!;
            }

            var user = await _manager.AuthService.GetOneUserAsync(User.Identity?.Name);

            var cacheOptions = new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(cacheKey, user, cacheOptions);

            if (user != null && user.AvatarUrl != null)
            {
                return user.AvatarUrl;
            }
            return "avatars/default.png";
        }
    }
}
