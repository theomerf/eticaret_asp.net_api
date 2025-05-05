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
        public IQueryable<Product> GetAllProducts(bool trackChanges) => FindAll(trackChanges);

        public int GetCount(bool trackChanges)
        {
            return Count(trackChanges);
        }

        public IQueryable<ProductWithRatingDto> GetFavouriteProducts(ICollection<int> favouriteProductIds, bool trackChanges)
        {
            return FindAllByCondition(p => favouriteProductIds.Contains(p.ProductId), trackChanges)
                .Select(p => new ProductWithRatingDto
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
                .OrderBy(p => p.ProductId);
        }

        public IQueryable<Product> GetAllProductsWithDetailsAdmin(ProductRequestParameters p)
        {
            return _context
                .Products
                .OrderBy(p => p.ProductId)
                .FilteredByCategoryId(p.CategoryId)
                .FilteredBySearchTerm(p.SearchTerm)
                .FilteredByPrice(p.MinPrice, p.MaxPrice, p.IsValidPrice)
                .ToPaginate(p.PageNumber, p.PageSize);
        }

        public IQueryable<ProductWithRatingDto> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            var filteredProducts = FindAllByCondition(prd =>
                (string.IsNullOrEmpty(p.SearchTerm) || prd.ProductName.ToLower().Contains(p.SearchTerm.ToLower())) &&
                (!p.CategoryId.HasValue || prd.MainCategoryId == p.CategoryId || prd.SubCategoryId == p.CategoryId) &&
                (!p.IsValidPrice || (prd.DiscountPrice >= p.MinPrice && prd.DiscountPrice <= p.MaxPrice))
            , trackChanges: false)
            .OrderBy(prd => prd.ProductId)
            .Skip((p.PageNumber - 1) * p.PageSize)
            .Take(p.PageSize);

            return filteredProducts.Select(p => new ProductWithRatingDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ImageUrl = p.ImageUrl,
                ShowCase = p.ShowCase,
                DiscountPrice = p.DiscountPrice,
                ActualPrice = p.ActualPrice,
                AverageRating = _context.UserReviews
                    .Where(r => r.ProductId == p.ProductId)
                    .Average(r => (double?)r.Rating) ?? 0
            });
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

        public IQueryable<ProductWithRatingDto> GetShowcaseProductsWithRatings(bool trackChanges)
        {
            return FindAllByCondition(p => p.ShowCase == true, trackChanges)
                .Select(p => new ProductWithRatingDto
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
                .OrderBy(p => p.ProductId);
        }

        public void UpdateOneProduct(Product entity) => Update(entity);
    }
}