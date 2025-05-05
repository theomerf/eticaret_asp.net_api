using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;

namespace Repositories.Contracts{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        IQueryable<Product> GetAllProducts(bool trackChanges);
        int GetCount(bool trackChanges);
        IQueryable<ProductWithRatingDto> GetFavouriteProducts(ICollection<int> favouriteProductIds, bool trackChanges);
        IQueryable<ProductWithRatingDto> GetAllProductsWithDetails(ProductRequestParameters p);
        IQueryable<Product> GetShowcaseProducts(bool trackChanges);
        Product? GetOneProduct(int id, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteOneProduct(Product product);
        void UpdateOneProduct(Product entity);
        IQueryable<Product> GetAllProductsWithDetailsAdmin(ProductRequestParameters p);
        IQueryable<ProductWithRatingDto> GetShowcaseProductsWithRatings(bool trackChanges);
    }
}