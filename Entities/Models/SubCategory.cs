using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public int MainCategoryId { get; set; }
        [JsonIgnore]
        public MainCategory MainCategory { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
