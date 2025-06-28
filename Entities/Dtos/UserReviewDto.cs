using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record UserReviewDto
    {
        public int UserReviewId { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public string ReviewTitle { get; set; } = string.Empty;
        public string? ReviewDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        [Required]
        [Range(1, 5, ErrorMessage = "Puanlama 1 ile 5 arasında olmalıdır.")]
        public int Rating { get; set; }
        public string? ReviewPictureUrl { get; set; }
    }
}
