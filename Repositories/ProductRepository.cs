using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges) => await FindAll(trackChanges).ToListAsync();

        public async Task<int> GetCountAsync(bool trackChanges) => await Count(trackChanges);

        public async Task<IEnumerable<Product>> GetFavouriteProductsAsync(ICollection<int> favouriteProductIds, bool trackChanges)
        {
            return await FindAllByCondition(p => favouriteProductIds.Contains(p.ProductId), trackChanges)
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ImageUrl = p.ImageUrl,
                    DiscountPrice = p.DiscountPrice,
                    ActualPrice = p.ActualPrice,
                    AverageRating = _context.UserReviews
                        .Where(r => r.ProductId == p.ProductId)
                        .Average(r => (double?)r.Rating) ?? 0
                })
                .OrderBy(p => p.ProductId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithDetailsAdminAsync(ProductRequestParameters p)
        {
            return await FindAll(false).
                OrderBy(p => p.ProductId)
                .ToListAsync();
        }

        public async Task<int> GetAllProductsCountWithDetailsAsync(ProductRequestParameters p)
        {
            var totalCount = await FindAll(false)
                .FilteredByCategoryId(p.MainCategoryId, p.SubCategoryId)
                .FilteredBySearchTerm(p.SearchTerm ?? "")
                .FilteredByPrice(p.MinPrice, p.MaxPrice, p.IsValidPrice)
                .FilteredByShowcase(p.IsShowCase)
                .FilteredByDiscount(p.IsDiscount)
                .CountAsync();

            return totalCount;
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithDetailsAsync(ProductRequestParameters p)
        {
            var filteredProducts = await FindAll(false)
                .FilteredByCategoryId(p.MainCategoryId, p.SubCategoryId)
                .FilteredBySearchTerm(p.SearchTerm ?? "")
                .FilteredByPrice(p.MinPrice, p.MaxPrice, p.IsValidPrice)
                .FilteredByShowcase(p.IsShowCase)
                .FilteredByDiscount(p.IsDiscount)
                .Include(p => p.UserReviews)
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ImageUrl = p.ImageUrl,
                    ActualPrice = p.ActualPrice,
                    DiscountPrice = p.DiscountPrice,
                    ShowCase = p.ShowCase,
                    AverageRating = _context.UserReviews
                        .Where(r => r.ProductId == p.ProductId)
                        .Average(r => (double?)r.Rating) ?? 0,
                    UserReviews = p.UserReviews.Select(r => new UserReview
                    {
                       Rating = r.Rating
                    }).ToList()
                })
                .OrderBy(prd => prd.ProductId)
                .ToPaginate(p.PageNumber, p.PageSize)
                .ToListAsync();

            return filteredProducts;
        }

        public async Task<Product?> GetOneProductAsync(int id, bool trackChanges) => await FindByCondition(p => p.ProductId.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetShowcaseProductsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
               .Where(p => p.ShowCase.Equals(true))
               .OrderBy(p => p.ProductId)
               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetShowcaseProductsWithRatingsAsync(bool trackChanges)
        {
            return await FindAllByCondition(p => p.ShowCase == true, trackChanges)
                .Include(p => p.UserReviews)
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ImageUrl = p.ImageUrl,
                    DiscountPrice = p.DiscountPrice,
                    ActualPrice = p.ActualPrice,
                    AverageRating = _context.UserReviews
                        .Where(r => r.ProductId == p.ProductId)
                        .Average(r => (double?)r.Rating) ?? 0,
                    UserReviews = p.UserReviews.Select(r => new UserReview
                    {
                        Rating = r.Rating
                    }).ToList()
                })
                .OrderBy(p => p.ProductId)
                .ToListAsync();
        }

        public void UpdateOneProduct(Product entity) => Update(entity);

        public async Task<IEnumerable<Product>> GetLastestProductsAsync(int n, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderByDescending(prd => prd.ProductId)
                .Take(n)
                .ToListAsync();
        }
    }
}