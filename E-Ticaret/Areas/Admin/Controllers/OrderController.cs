using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;


        public OrderController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var orders = _manager.OrderService.Orders;
            foreach (var order in orders)
            {
                foreach (var line in order.Lines)
                {
                    var product = _manager.ProductService.GetOneProduct(line.ProductId, false);
                    if (product != null)
                    {
                        line.ProductName = product.ProductName;
                        line.ImageUrl = product.ImageUrl;
                        line.DiscountPrice = product.DiscountPrice;
                        line.ActualPrice = product.ActualPrice;
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Ürün bulunamadı.");
                    }
                }
            }
            return View(orders);
        }

        [HttpPost]
        public IActionResult Complete([FromForm] int id)
        {
            _manager.OrderService.Complete(id);
            return RedirectToAction("Index");
        }
    }


}
