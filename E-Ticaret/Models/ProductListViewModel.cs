using Entities.Dtos;
using Entities.Models;

namespace ETicaret.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductWithRatingDto> Products { get; set; }
        public Pagination Pagination { get; set; }
        public int TotalCount => Products.Count();
    }
}
