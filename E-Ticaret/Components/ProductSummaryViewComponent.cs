using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Threading.Tasks;

namespace ETicaret.Components
{
    public class ProductSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ProductSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<String> InvokeAsync()
        {
            var productsCount = await _manager.ProductService.GetCountAsync(false);
            return productsCount.ToString();
        }
    }
}
