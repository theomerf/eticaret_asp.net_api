using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class OrderInProgressViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        public OrderInProgressViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }
        public async Task<string> InvokeAsync()
        {
            var orders = await _manager.OrderService.GetAllOrdersAsync();
            return orders.Count().ToString();
        }
    }
}
