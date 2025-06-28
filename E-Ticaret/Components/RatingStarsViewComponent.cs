using Entities.Dtos;
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

        public IViewComponentResult Invoke(string mode, int? ratingsCount = null, double? averageRating = null, IEnumerable<UserReview>? userReviews = null, IEnumerable<UserReviewDto>? userReviewsDto = null)
        {
           /* IEnumerable<int> ratings = await _manager.UserReviewService.GetAllRatingsForProductAsync(productId, false);*/
            if (mode == "ratings"){
                return View(userReviewsDto);
            }
            else if(mode == "average")
            {
                return View("average", averageRating);
            }
            else if (mode == "count")
            {
                return View("count", ratingsCount);
            }
            else if (mode == "stars")
            {
                return View("stars", averageRating);
            }
            else if (mode == "starsForCard")
            {
                return View("starsForCard", userReviews);
            }
            else
            {
                return Content("Invalid mode");
            }

        }
    }
}
