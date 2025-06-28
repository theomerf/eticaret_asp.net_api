using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;
using ETicaret.Models;
using System.ComponentModel;

namespace ETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index([FromQuery] ProductRequestParameters p)
        {
            ViewData["Title"] = "Products";
            var products =  await _manager.ProductService.GetAllProductsWithDetailsAdminAsync(p);
            var paginaton = new Pagination()
            {
                CurrentPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = products.Count()
            };
            var productlist = new ProductListViewModelAdmin()
            {
                Products = products,
                Pagination = paginaton
            };
            return View(new ProductDtoForShowcaseUpdate()
            {
                ProductList = productlist,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ProductDtoForUpdate productDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.ProductService.UpdateOneProductAsync(productDto);
                if (productDto.ShowCase) 
                {
                    TempData["success"] = $"{productDto.ProductName} adlı ürün başarıyla vitrine eklendi.";
                }
                else
                {
                    TempData["success"] = $"{productDto.ProductName} adlı ürün başarıyla vitrinden kaldırıldı.";
                }

                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await GetMainCategoriesSelectList();
            ViewBag.SubCategories = await GetSubCategoriesSelectList();
            return View();
        }

        public async Task<SelectList> GetMainCategoriesSelectList()
        {
            var categories = await _manager.MainCategoryService.GetAllCategoriesAsync(false);
            return new SelectList(categories, "MainCategoryId", "CategoryName", "1");

        }
        public async Task<SelectList> GetSubCategoriesSelectList()
        {
            var categories = await _manager.SubCategoryService.GetAllCategoriesAsync(false);
            return new SelectList(categories, "SubCategoryId", "CategoryName", "1");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var products = await _manager.ProductService.GetAllProductsAsync(false);

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images",$"{(products.Count()+1).ToString()}.png");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                productDto.ImageUrl = (products.Count() + 1).ToString() + ".png";
                await _manager.ProductService.CreateProductAsync(productDto);
                TempData["success"] = $"{productDto.ProductName} adlı ürün başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Bir şeyler yanlış gitti";
                return View();
            }


        }

        public async Task<IActionResult> Update([FromRoute(Name = "id")] int id)
        {
            ViewBag.MainCategories = await GetMainCategoriesSelectList();
            ViewBag.SubCategories = await GetSubCategoriesSelectList();
            var model = await _manager.ProductService.GetOneProductForUpdateAsync(id, true);
            ViewData["Title"] = model?.ProductName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] ProductDtoForUpdate productDto, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    productDto.ImageUrl = file.FileName;
                }

                await _manager.ProductService.UpdateOneProductAsync(productDto);
                TempData["success"] = $"{productDto.ProductName} adlı ürün başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute(Name ="id")] int id)
        {
            await _manager.ProductService.DeleteOneProductAsync(id);
            TempData["success"] = "Ürün başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
