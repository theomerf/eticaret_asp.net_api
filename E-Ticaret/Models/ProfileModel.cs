using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Models
{
    public class ProfileModel
    {
        public required UserDto User { get; set; }
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public IEnumerable<UserReviewDto> UserReviews { get; set; } = new List<UserReviewDto>();
        public required UserDtoForUpdate UserDtoForUpdate { get; set;}
        public ChangePasswordDto? ChangePasswordDto { get; set; }
    }
}
