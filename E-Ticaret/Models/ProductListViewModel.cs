using Entities.Models;

namespace ETicaret.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public Pagination Pagination { get; set; }
        public int TotalCount => Products.Count();
    }
}
