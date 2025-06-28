using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool trackChanges);
        Task<int> GetCountAsync(bool trackChanges);
        Task<IEnumerable<ProductDto>> GetLastestProductsAsync(int n, bool trackChanges);
        Task<IEnumerable<ProductWithRatingDto>> GetFavouriteProductsAsync(UserDtoForFavourites favouritesDto, bool trackChanges);
        Task<int> GetAllProductsCountWithDetailsAsync(ProductRequestParameters p);
        Task<IEnumerable<ProductWithRatingDto>> GetAllProductsWithDetailsAsync(ProductRequestParameters p);
        Task<IEnumerable<ProductDto>> GetAllProductsWithDetailsAdminAsync(ProductRequestParameters p);
        Task<IEnumerable<ProductDto>> GetShowcaseProductsAsync(bool trackChanges);
        Task<ProductDto> GetOneProductAsync(int id, bool trackChanges);
        Task CreateProductAsync(ProductDtoForInsertion productDto);
        Task UpdateOneProductAsync(ProductDtoForUpdate productDto);
        Task DeleteOneProductAsync(int id);
        Task<ProductDtoForUpdate> GetOneProductForUpdateAsync(int id, bool trackChanges);
        Task<IEnumerable<ProductWithRatingDto>> GetShowcaseProductsWithRatingsAsync(bool trackChanges);
    }
}
