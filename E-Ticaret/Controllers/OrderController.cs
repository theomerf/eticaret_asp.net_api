using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;

        private readonly UserManager<Entities.Models.Account> _userManager;

        private readonly Cart _cart;

        public OrderController(IServiceManager manager, Cart cart, UserManager<Entities.Models.Account> userManager)
        {
            _manager = manager;
            _cart = cart;
            _userManager = userManager;
        }

        [Authorize]
        public ViewResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([FromForm] Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid) 
            {
                order.Lines = _cart.Lines.ToArray();
                order.UserName = _userManager.FindByNameAsync(User.Identity.Name).Result.UserName;
                _manager.OrderService.SaveOrder(order);
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
