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

        public IActionResult Index([FromQuery] ProductRequestParameters p)
        {
            ViewData["Title"] = "Products";
            var products = _manager.ProductService.GetAllProductsWithDetails(p);
            var paginaton = new Pagination()
            {
                CurrentPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = _manager.ProductService.GetAllProducts(false).Count()
            };
            return View(new ProductListViewModel()
            {
                Products = products,
                Pagination = paginaton
            });
        }

        public IActionResult Create()
        {
            ViewBag.MainCategories = GetMainCategoriesSelectList();
            ViewBag.SubCategories = GetSubCategoriesSelectList();
            return View();
        }

        public SelectList GetMainCategoriesSelectList()
        {
            return new SelectList(_manager.MainCategoryService.GetAllCategories(false), "MainCategoryId", "CategoryName", "1");

        }
        public SelectList GetSubCategoriesSelectList()
        {
            return new SelectList(_manager.SubCategoryService.GetAllCategories(false), "SubCategoryId", "CategoryName", "1");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images",$"{(_manager.ProductService.GetAllProducts(false).Count()+1).ToString()}.png");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                productDto.ImageUrl = (_manager.ProductService.GetAllProducts(false).Count() + 1).ToString() + ".png";
                _manager.ProductService.CreateProduct(productDto);
                TempData["success"] = $"{productDto.ProductName} has been created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Something went wrong";
                return View();
            }


        }

        public IActionResult Update([FromRoute(Name = "id")] int id)
        {
            ViewBag.MainCategories = GetMainCategoriesSelectList();
            ViewBag.SubCategories = GetSubCategoriesSelectList();
            var model = _manager.ProductService.GetOneProductForUpdate(id, false);
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

                _manager.ProductService.UpdateOneProduct(productDto);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete([FromRoute(Name ="id")] int id)
        {
            _manager.ProductService.DeleteOneProduct(id);
            return RedirectToAction("Index");
        }
    }
}
