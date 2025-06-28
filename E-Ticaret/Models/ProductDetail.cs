using Entities.Dtos;
using Entities.Models;

namespace ETicaret.Models
{
    public record ProductDetail
    {
        public UserReviewDtoForInsertion? UserReviewDtoForInsertion { get; set; }
        public ProductDto? Product { get; set; }
        public IEnumerable<UserReviewDto>? UserReviews { get; set; } = new List<UserReviewDto>();
    }
}
