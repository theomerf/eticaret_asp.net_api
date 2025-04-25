using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Models
{
    public class ProfileModel
    {
        public Account User { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public IEnumerable<UserReview> UserReviews { get; set; } = new List<UserReview>();
        public UserReviewDtoForUpdate UserReviewDtoForUpdate { get; set; }
        public UserDtoForUpdate UserDtoForUpdate { get; set;}
        public ChangePasswordDto ChangePasswordDto { get; set; }
    }
}
