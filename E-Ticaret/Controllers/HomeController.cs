using Microsoft.AspNetCore.Mvc;

namespace ETicaret.Controllers{
    public class HomeController : Controller{
        public IActionResult Index(){
            ViewData["Title"] = "Welcome";
            return View();
        } 
    }
}