using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            _cart = cartService;
        }

        public IViewComponentResult Invoke() 
        {
            var cartSummary = _cart.Lines.Count().ToString();

            return Content(cartSummary); 
        }
    }
}
