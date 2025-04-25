using Entities.Dtos;

namespace ETicaret.Models
{
    public record ProductDtoForShowcaseUpdate : ProductDto
    {
        public ProductDtoForUpdate? ProductDtoForUpdate { get; init; }
        public ProductListViewModel? ProductList { get; init; }
    }
}
