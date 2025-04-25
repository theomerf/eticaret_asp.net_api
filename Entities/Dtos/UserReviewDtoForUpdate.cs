using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record UserReviewDtoForUpdate : UserReviewDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Puanlama 1 ile 5 arasında olmalıdır.")]
        public int Rating { get; set; }
        public string? ReviewUpdateDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
