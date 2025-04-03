namespace Entities.Models
{
    public class MainCategory
    {
        public int MainCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
