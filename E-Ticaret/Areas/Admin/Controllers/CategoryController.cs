using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace ETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {

        private IRepositoryManager _manager;

        public CategoryController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var model = _manager.MainCategory.FindAll(false);
            return View(model);
        }

    }
}
