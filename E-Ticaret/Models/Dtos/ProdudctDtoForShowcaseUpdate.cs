using Entities.Dtos;

namespace ETicaret.Models
{
    public record ProductDtoForShowcaseUpdate : ProductDto
    {
        public ProductDtoForUpdate? ProductDtoForUpdate { get; init; }
        public ProductListViewModelAdmin? ProductList { get; init; }
    }
}
