using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using Entities.RequestParameters;
using ETicaret.Models;

namespace ETicaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager){
            _manager = manager;
        }

        public IActionResult Index(ProductRequestParameters p)
        {
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

        public IActionResult Get([FromRoute(Name="id")] int id){
            var model = _manager.ProductService.GetOneProduct(id, false);
            ViewData["Title"] = model?.ProductName;
            return View(model);
        }
    }
}