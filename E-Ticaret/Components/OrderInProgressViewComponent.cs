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
        public string Invoke()
        {
            var orders = _manager.OrderService.Orders;
            return orders.ToList().Count.ToString();
        }
    }
}
