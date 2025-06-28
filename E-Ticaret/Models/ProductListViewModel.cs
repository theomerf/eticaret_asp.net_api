using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;

namespace ETicaret.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductWithRatingDto>? Products { get; set; }
        public Pagination? Pagination { get; set; }
        public int? TotalCount => Products?.Count();
    }
}
