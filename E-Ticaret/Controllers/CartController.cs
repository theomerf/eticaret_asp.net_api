using Entities.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Controllers
{
    public class CartController : Controller
    {
        private readonly IServiceManager _manager;

        private readonly Cart _cart;

        public CartController(IServiceManager manager, Cart cartService)
        {
            _manager = manager;
            _cart = cartService;
        }

        public IActionResult Index(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(_cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string returnUrl)
        {
            Product? product = _manager.ProductService.GetOneProduct(productId, false);

            if (product != null)
            {
                _cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id, string returnUrl)
        {
            var line = _cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == id);
            if (line != null)
            {
                _cart.RemoveLine(line.Product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int id, string returnUrl, int itemQuantity)
        {
            var line = _cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == id);
            if (line != null)
            {
                _cart.DecreaseQuantity(line.Product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int id, string returnUrl)
        {
            var line = _cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == id);
            if (line != null)
            {
                _cart.IncreaseQuantity(line.Product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }



    }
}
