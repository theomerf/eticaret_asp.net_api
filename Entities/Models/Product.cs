namespace Entities.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public String? ProductName { get; set; } = string.Empty;
        public int MainCategoryId { get; set; }
        public MainCategory? MainCategory { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        public String? ImageUrl { get; set; }
        public decimal? DiscountPrice { get; set; } = 0;
        public decimal ActualPrice { get; set; }
        public String? Summary { get; set; } = String.Empty;
        public int Rating { get; set; } = 4;
        public int Discount => DiscountPrice.HasValue && DiscountPrice.Value > 0
           ? (int)((1 - (DiscountPrice.Value / ActualPrice)) * 100)
           : 0;
        public bool ShowCase { get; set; } = false;
        public ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
    }
}

