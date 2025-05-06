using Entities.Dtos;
using Entities.Models;
using ETicaret.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

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
            var cart = SessionCart.GetCart(HttpContext.RequestServices);
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var cartDto = SessionCart.GetCartDto(HttpContext.RequestServices);
                cartDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _manager.CartService.AddOrUpdateCart(cartDto,userId);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(cart);
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
            var line = _cart.Lines.FirstOrDefault(cl => cl.ProductId == id);
            if (line != null)
            {
                _cart.RemoveLine(id);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public IActionResult RemoveFromCartAjax(int productId)
        {
            Product? product = _manager.ProductService.GetOneProduct(productId, false);
            if (product != null)
            {
                _cart.RemoveLine(productId);
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
            var line = _cart.Lines.FirstOrDefault(cl => cl.ProductId == id);
            if (line != null)
            {
                _cart.DecreaseQuantity(line.ProductId, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int id, string returnUrl)
        {
            var line = _cart.Lines.FirstOrDefault(cl => cl.ProductId == id);
            if (line != null)
            {
                _cart.IncreaseQuantity(line.ProductId, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }



    }
}
