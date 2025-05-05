using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(bool trackChanges);
        int GetCount(bool trackChanges);
        IEnumerable<Product> GetLastestProducts(int n, bool trackChanges);
        IEnumerable<ProductWithRatingDto> GetFavouriteProducts(ICollection<int> user, bool trackChanges);
        IEnumerable<ProductWithRatingDto> GetAllProductsWithDetails(ProductRequestParameters p);
        IEnumerable<Product> GetAllProductsWithDetailsAdmin(ProductRequestParameters p);
        IEnumerable<Product> GetShowcaseProducts(bool trackChanges);
        Product? GetOneProduct(int id, bool trackChanges);
        void CreateProduct(ProductDtoForInsertion productDto);
        void UpdateOneProduct(ProductDtoForUpdate productDto);
        void DeleteOneProduct(int id);
        ProductDtoForUpdate GetOneProductForUpdate(int id, bool trackChanges);
        IEnumerable<ProductWithRatingDto> GetShowcaseProductsWithRatings(bool trackChanges);
    }
}
