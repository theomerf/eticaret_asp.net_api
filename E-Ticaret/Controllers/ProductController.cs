using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Entities.RequestParameters;
using ETicaret.Models;
using Entities.Dtos;

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
            var products = _manager.ProductService.GetAllProductsWithDetails(p).ToList();
            var totalCount = _manager.ProductService.GetAllProducts(false).Count();

            var pagination = new Pagination()
            {
                CurrentPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = totalCount
            };

            return View(new ProductListViewModel()
            {
                Products = products,
                Pagination = pagination
            });
        }


        public IActionResult Get([FromRoute(Name="id")] int id){
            var product = _manager.ProductService.GetOneProduct(id, false);
            ViewData["Title"] = product?.ProductName;
            var reviews = _manager.UserReviewService.GetAllUserReviewsOfOneProduct(id, false).ToList();
            return View(new ProductDetail()
            {
                Product = product,
                UserReviews = reviews
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Get([FromForm] ProductDetail model, IFormFile file = null) { 
            if(model.UserReviewDtoForInsertion != null)
            {
                if (ModelState.IsValid)
                {
                    if (file != null) 
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/comments", $"{(_manager.UserReviewService.GetAllUserReviews(false).Count() + 1).ToString()}.png");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    var user = await _manager.AuthService.GetOneUser(User.Identity.Name);
                    var review = new UserReviewDtoForInsertion
                    {
                        ProductId = model.Product.ProductId,
                        UserId = user.Id,
                        ReviewTitle = model.UserReviewDtoForInsertion.ReviewTitle,
                        ReviewText = model.UserReviewDtoForInsertion.ReviewText,
                        Rating = model.UserReviewDtoForInsertion.Rating,
                        ReviewPictureUrl = $"{(_manager.UserReviewService.GetAllUserReviews(false).Count() + 1).ToString()}.png"
                    };

                    _manager.UserReviewService.CreateUserReview(review);
                    TempData["success"] = "Yorumunuz baþarýyla eklendi.";
                    return RedirectToAction("Get");

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