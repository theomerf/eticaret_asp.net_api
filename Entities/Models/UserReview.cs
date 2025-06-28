using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class UserReview
    {
        public int UserReviewId { get; set; } 
        public int Rating { get; set; } 
        public string? ReviewTitle { get; set; }
        public string? ReviewText { get; set; }
        public string? ReviewDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string? ReviewUpdateDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string? ReviewPictureUrl { get; set; } = string.Empty;
        public string? UserId { get; set; } = string.Empty; 
        public Account? User { get; set; } 
        public int ProductId { get; set; } 
        public Product? Product { get; set; }
    }
}
