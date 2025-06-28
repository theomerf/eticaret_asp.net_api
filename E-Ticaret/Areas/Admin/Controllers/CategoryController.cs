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

        public async Task<IActionResult> Index()
        {
            var model = await _manager.MainCategoryService.GetAllCategoriesAsync(false);
            return View(model);
        }

        public IActionResult MainCategoryCreate()
        {
            return View(new MainCategoryDtoForInsertion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainCategoryCreate([FromForm] MainCategoryDtoForInsertion categoryDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.MainCategoryService.CreateCategoryAsync(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Bir şeyler yanlış gitti";
                return View();
            }
        }

        public async Task<IActionResult> MainCategoryUpdate([FromRoute(Name = "id")] int id)
        {
            var model = await _manager.MainCategoryService.GetOneCategoryForUpdateAsync(id, false);
            ViewData["Title"] = model?.CategoryName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainCategoryUpdate([FromForm] MainCategoryDtoForUpdate categoryDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.MainCategoryService.UpdateCategoryAsync(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> MainCategoryDelete([FromRoute(Name = "id")] int id)
        {
            await _manager.MainCategoryService.DeleteOneCategoryAsync(id);
            TempData["success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("Index");
        }

        public IActionResult SubCategoryCreate([FromQuery(Name = "mainCategoryId")] int mainCategoryId)
        {
            var model = new SubCategoryDtoForInsertion
            {
                MainCategoryId = mainCategoryId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubCategoryCreate([FromForm] SubCategoryDtoForInsertion categoryDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.SubCategoryService.CreateCategoryAsync(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Bir şeyler yanlış gitti";
                return View();
            }
        }

        public async Task<IActionResult> SubCategoryUpdate([FromRoute(Name = "id")] int id)
        {
            var model = await _manager.SubCategoryService.GetOneCategoryForUpdateAsync(id, false);
            ViewData["Title"] = model?.CategoryName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubCategoryUpdate([FromForm] SubCategoryDtoForUpdate categoryDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.SubCategoryService.UpdateCategoryAsync(categoryDto);
                TempData["success"] = $"{categoryDto.CategoryName} adlı kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> SubCategoryDelete([FromRoute(Name = "id")] int id)
        {
            await _manager.SubCategoryService.DeleteOneCategoryAsync(id);
            TempData["success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("Index");
        }

    }
}
