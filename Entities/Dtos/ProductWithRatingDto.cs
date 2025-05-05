using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record ProductWithRatingDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal? DiscountPrice { get; set; } = 0;
        public decimal ActualPrice { get; set; }
        public bool ShowCase { get; set; } = false;
        public double AverageRating { get; set; }
        public int Discount => DiscountPrice.HasValue && DiscountPrice.Value > 0
           ? (int)((1 - (DiscountPrice.Value / ActualPrice)) * 100)
           : 0;
        public bool isFavourite { get; set; } = false;
    }
}
