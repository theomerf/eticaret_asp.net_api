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

        public async Task<IActionResult> Index()
        {
            var orders = await _manager.OrderService.GetAllOrdersAsync();
            foreach (var order in orders)
            {
                foreach (var line in order.Lines)
                {
                    var product = await _manager.ProductService.GetOneProductAsync(line.ProductId, false);
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
        public async Task<IActionResult> Complete([FromForm] int id)
        {
            await _manager.OrderService.CompleteAsync(id);
            return RedirectToAction("Index");
        }
    }


}
