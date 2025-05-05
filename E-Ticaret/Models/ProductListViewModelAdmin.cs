using Entities.Models;

namespace ETicaret.Models
{
    public class ProductListViewModelAdmin
    {
        public IEnumerable<Product> Products { get; set; }
        public Pagination Pagination { get; set; }
        public int TotalCount => Products.Count();
    }
}
