using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;

namespace Repositories.Contracts{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        Task<int> GetCountAsync(bool trackChanges);
        Task<IEnumerable<Product>> GetFavouriteProductsAsync(ICollection<int> favouriteProductIds, bool trackChanges);
        Task<int> GetAllProductsCountWithDetailsAsync(ProductRequestParameters p);
        Task<IEnumerable<Product>> GetAllProductsWithDetailsAsync(ProductRequestParameters p);
        Task<IEnumerable<Product>> GetShowcaseProductsAsync(bool trackChanges);
        Task<Product?> GetOneProductAsync(int id, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteOneProduct(Product product);
        void UpdateOneProduct(Product entity);
        Task<IEnumerable<Product>> GetAllProductsWithDetailsAdminAsync(ProductRequestParameters p);
        Task<IEnumerable<Product>> GetShowcaseProductsWithRatingsAsync(bool trackChanges);
        Task<IEnumerable<Product>> GetLastestProductsAsync(int n, bool trackChanges);
    }
}