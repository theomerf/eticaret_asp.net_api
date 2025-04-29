using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Repositories.Extensions;

namespace Repositories
{
    public sealed class ProductRepository : RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void DeleteOneProduct(Product product) => Remove(product);
        public IQueryable<Product> GetAllProducts(bool trackChanges) => FindAll(trackChanges);

        public IQueryable<Product> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            return _context
                .Products
                .OrderBy(p => p.ProductId)
                .FilteredByCategoryId(p.CategoryId)
                .FilteredBySearchTerm(p.SearchTerm)
                .FilteredByPrice(p.MinPrice, p.MaxPrice, p.IsValidPrice)
                .ToPaginate(p.PageNumber, p.PageSize); 
        }

        public Product? GetOneProduct(int id, bool trackChanges)
        {
            return FindByCondition(p => p.ProductId.Equals(id), trackChanges);
        }

        public IQueryable<Product> GetShowcaseProducts(bool trackChanges)
        {
            return FindAll(trackChanges)
               .Where(p => p.ShowCase.Equals(true))
               .OrderBy(p => p.ProductId);
        }

        public void UpdateOneProduct(Product entity) => Update(entity);
    }
}