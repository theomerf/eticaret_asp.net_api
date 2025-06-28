using Entities.Dtos;
using Entities.RequestParameters;
using ETicaret.Infrastructure.CookieHelpler;
using ETicaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Security.Claims;

namespace ETicaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager){
            _manager = manager;
        }

        public async Task<IActionResult> Index(ProductRequestParameters p)
        {
            var products = await _manager.ProductService.GetAllProductsWithDetailsAsync(p);
            var totalCount = await _manager.ProductService.GetAllProductsCountWithDetailsAsync(p);
            var favIds = CookieHelper.GetFavouriteProductIds(Request);
            ViewBag.FavouriteIds = CookieHelper.GetFavouriteProductIds(Request);

            var pagination = new Pagination
            {
                CurrentPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = totalCount
            };

            var model = new ProductListViewModel
            {
                Products = products,
                Pagination = pagination,
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" && Request.Headers["X-Filter-Request"] == "true")
            {
                var html = await this.RenderViewAsync("_ProductListPartial", model, true);
                return Json(new { success = true, html = html, totalCount = totalCount });
            }

            return View(model);
        }



        public async Task<IActionResult> Get([FromRoute(Name = "id")] int id)
        {
            var product = await _manager.ProductService.GetOneProductAsync(id, false);
            ViewData["Title"] = product?.ProductName;
            var reviews = await _manager.UserReviewService.GetAllUserReviewsOfOneProductAsync(id, false);
            return View(new ProductDetail()
            {
                Product = product,
                UserReviews = reviews,
                UserReviewDtoForInsertion = new UserReviewDtoForInsertion() 
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Get([FromForm] ProductDetail model, IFormFile? file = null) { 
            if(model.UserReviewDtoForInsertion != null && model.Product != null)
            {
                if (ModelState.IsValid)
                {
                    var reviewCount = await _manager.UserReviewService.GetCountAsync(false);

                    if (file != null) 
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/comments", $"{(reviewCount + 1).ToString()}.png");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var reviewWithPicture = new UserReviewDtoForInsertion
                        {
                            ProductId = model.Product.ProductId,
                            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                            ReviewTitle = model.UserReviewDtoForInsertion.ReviewTitle,
                            ReviewText = model.UserReviewDtoForInsertion.ReviewText,
                            Rating = model.UserReviewDtoForInsertion.Rating,
                            ReviewPictureUrl = $"{(reviewCount + 1).ToString()}.png"
                        };


                        await _manager.UserReviewService.CreateUserReviewAsync(reviewWithPicture);
                        TempData["success"] = "Yorumunuz baþarýyla eklendi.";
                        return RedirectToAction("Get");
                    }
                    else
                    {
                        var review = new UserReviewDtoForInsertion
                        {
                            ProductId = model.Product.ProductId,
                            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                            ReviewTitle = model.UserReviewDtoForInsertion.ReviewTitle,
                            ReviewText = model.UserReviewDtoForInsertion.ReviewText,
                            Rating = model.UserReviewDtoForInsertion.Rating,
                        };

                        await _manager.UserReviewService.CreateUserReviewAsync(review);
                        TempData["success"] = "Yorumunuz baþarýyla eklendi.";
                        return RedirectToAction("Get");
                    }

                }
            }
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    throw new Exception($"Property: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

            return RedirectToAction("Get");
        }
    }
}