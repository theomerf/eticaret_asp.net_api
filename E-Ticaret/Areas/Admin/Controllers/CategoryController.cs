using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Services.Contracts;

namespace ETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {

        private readonly IServiceManager _manager;

        public CategoryController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var model = _manager.MainCategoryService.GetAllCategories(false);
            return View(model);
        }

        public async Task<IActionResult> MainCategoryCreate()
        {
            return View(new MainCategoryDtoForInsertion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MainCategoryCreate([FromForm] MainCategoryDtoForInsertion categoryDto)
        {
            if (ModelState.IsValid)
            {
                _manager.MainCategoryService.CreateCategory(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Bir şeyler yanlış gitti";
                return View();
            }
        }

        public IActionResult MainCategoryUpdate([FromRoute(Name = "id")] int id)
        {
            var model = _manager.MainCategoryService.GetOneCategoryForUpdate(id, false);
            ViewData["Title"] = model?.CategoryName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainCategoryUpdate([FromForm] MainCategoryDtoForUpdate categoryDto)
        {
            if (ModelState.IsValid)
            {
                _manager.MainCategoryService.UpdateCategory(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult MainCategoryDelete([FromRoute(Name = "id")] int id)
        {
            _manager.MainCategoryService.DeleteOneCategory(id);
            TempData["success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SubCategoryCreate([FromQuery(Name = "mainCategoryId")] int mainCategoryId)
        {
            var model = new SubCategoryDtoForInsertion
            {
                MainCategoryId = mainCategoryId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubCategoryCreate([FromForm] SubCategoryDtoForInsertion categoryDto)
        {
            if (ModelState.IsValid)
            {
                _manager.SubCategoryService.CreateCategory(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Bir şeyler yanlış gitti";
                return View();
            }
        }

        public IActionResult SubCategoryUpdate([FromRoute(Name = "id")] int id)
        {
            var model = _manager.SubCategoryService.GetOneCategoryForUpdate(id, false);
            ViewData["Title"] = model?.CategoryName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubCategoryUpdate([FromForm] SubCategoryDtoForUpdate categoryDto)
        {
            if (ModelState.IsValid)
            {
                _manager.SubCategoryService.UpdateCategory(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult SubCategoryDelete([FromRoute(Name = "id")] int id)
        {
            _manager.SubCategoryService.DeleteOneCategory(id);
            TempData["success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("Index");
        }

    }
}
