using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Components
{
    public class RatingStarsViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public RatingStarsViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public IViewComponentResult Invoke(int productId,double averageRating,string mode)
        {
            if (mode == "ratings"){
                IEnumerable<int> ratings = _manager.UserReviewService.GetAllRatingsForProduct(productId, false);
                return View(ratings);
            }
            else if(mode == "average")
            {;
                return View("average", averageRating);
            }
            else if (mode == "count")
            {
                IEnumerable<int> ratings = _manager.UserReviewService.GetAllRatingsForProduct(productId, false);
                return View("count", ratings);
            }
            else if (mode == "stars")
            {
                return View("stars", averageRating);
            }
            else
            {
                return Content("Invalid mode");
            }

        }
    }
}
