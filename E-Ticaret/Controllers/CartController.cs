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

        // Add these methods to your CartController.cs file

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

        // New AJAX version of AddToCart
        [HttpPost]
        public IActionResult AddToCartAjax(int productId)
        {
            Product? product = _manager.ProductService.GetOneProduct(productId, false);
            if (product != null)
            {
                _cart.AddItem(product, 1);
                return Json(new
                {
                    success = true,
                    message = $"{product.ProductName} sepete eklendi.",
                    type = "success"
                });
            }

            return Json(new
            {
                success = false,
                message = "Ürün bulunamadı.",
                type = "danger"
            });
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

        // Remove from cart method for AJAX
        [HttpPost]
        public IActionResult RemoveFromCartAjax(int productId)
        {
            Product? product = _manager.ProductService.GetOneProduct(productId, false);
            if (product != null)
            {
                _cart.RemoveLine(product);
                return Json(new
                {
                    success = true,
                    message = $"{product.ProductName} sepetten çıkarıldı.",
                    type = "success"
                });
            }

            return Json(new
            {
                success = false,
                message = "Ürün bulunamadı.",
                type = "danger"
            });
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            // Ensure this is an AJAX request to prevent direct access
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                return Forbid();
            }

            var cartItemCount = _cart.Lines.Count().ToString();
            return Json(cartItemCount);
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
