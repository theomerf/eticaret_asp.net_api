using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record UserReviewDtoForInsertion : UserReviewDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Puanlama 1 ile 5 arasında olmalıdır.")]
        public int Rating { get; set; }
        public string? ReviewDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
