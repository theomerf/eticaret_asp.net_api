using Entities.Dtos;
using Entities.Models;

namespace ETicaret.Models
{
    public record ProductDetail : UserReviewDto
    {
        public UserReviewDtoForInsertion? UserReviewDtoForInsertion { get; set; }
        public Product? Product { get; set; }
        public IEnumerable<UserReview>? UserReviews { get; set; } = new List<UserReview>();
    }
}
