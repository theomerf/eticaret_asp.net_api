using Microsoft.AspNetCore.Mvc;

namespace ETicaret.Components
{
    public class ProductFilterMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
