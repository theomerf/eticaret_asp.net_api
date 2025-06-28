using Entities.Dtos;
using Entities.Models;
using ETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

namespace ETicaret.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly Cart _cart;

        public OrderController(IServiceManager manager, Cart cart)
        {
            _manager = manager;
            _cart = cart;
        }

        [Authorize]
        public ViewResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([FromForm] OrderDto order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Üzgünüm, sepetiniz boş.");
            }
            if (ModelState.IsValid) 
            {
                foreach (var line in _cart.Lines)
                {
                    order.Lines.Add(new OrderLine
                    {
                        ProductId = line.ProductId,
                        Quantity = line.Quantity
                    });
                }
                order.UserName = User.FindFirstValue(ClaimTypes.Name);
                await _manager.OrderService.SaveOrderAsync(order);
                var cartDto = SessionCart.GetCartDto(HttpContext.RequestServices);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _manager.CartService.AddOrUpdateCartAsync(cartDto, userId);
                _cart.Clear();
                return RedirectToAction("Complete", new { orderId = order.OrderId });

            }
            else
            {
                return View(order);
            }
        }

        public IActionResult Complete(string orderId) 
        {
            return View((object)orderId);
        }

    }
}
