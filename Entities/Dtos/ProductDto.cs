using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProductDto
    {
        public int ProductId { get; init; }
        [Required(ErrorMessage = "ProductName is required.")]
        public string? ProductName { get; init; } = string.Empty;
        [Required(ErrorMessage = "Price is required")]
        public decimal ActualPrice { get; init; }
        public decimal DiscountPrice { get; init; }
        public String? Summary { get; init; } = String.Empty;
        public String? ImageUrl { get; set; }
        public int MainCategoryId { get; init; }
        public int SubCategoryId { get; init; }
    }
}
