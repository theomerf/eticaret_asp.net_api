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
        public decimal? DiscountPrice { get; init; }
        public int Discount => DiscountPrice.HasValue && DiscountPrice.Value > 0
        ? (int)((1 - (DiscountPrice.Value / ActualPrice)) * 100) : 0;
        public String? Summary { get; init; } = String.Empty;
        public String? ImageUrl { get; set; }
        public int MainCategoryId { get; init; }
        public int SubCategoryId { get; init; }
        public bool ShowCase { get; init; }
    }
}
