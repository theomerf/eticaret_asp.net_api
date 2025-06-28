using Entities.Dtos;
using Entities.Models;

namespace ETicaret.Models
{
    public class ProductListViewModelAdmin
    {
        public IEnumerable<ProductDto>? Products { get; set; }
        public Pagination? Pagination { get; set; }
        public int? TotalCount => Products?.Count();
    }
}
