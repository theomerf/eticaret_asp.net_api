using Microsoft.AspNetCore.Mvc;

namespace ETicaret.Controllers{
    public class HomeController : Controller{
        public IActionResult Index(){
            ViewData["Title"] = "Welcome";
            return View();
        }

        [HttpGet]
        public IActionResult PartialNavbar()
        {
            return PartialView("_Navbar");
        }
    }
}