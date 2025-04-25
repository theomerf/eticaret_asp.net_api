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

        public IViewComponentResult Invoke(int productId,string mode)
        {
            IEnumerable<UserReview> ratings = _manager.UserReviewService.GetAllUserReviewsOfOneProduct(productId, false);
            if (mode == "ratings"){
                return View(ratings);
            }
            else if(mode == "average")
            {;
                return View("average",ratings);
            }
            else if (mode == "count")
            {
                return View("count", ratings);
            }
            else if (mode == "stars")
            {
                return View("stars", ratings);
            }
            else
            {
                return Content("Invalid mode");
            }

        }
    }
}
