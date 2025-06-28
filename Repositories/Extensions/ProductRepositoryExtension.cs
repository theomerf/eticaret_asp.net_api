using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Extensions
{
    public static class ProductRepositoryExtension
    {
        public static IQueryable<Product> FilteredByCategoryId(this IQueryable<Product> products, int? mainCategoryId, int? subCategoryId)
        {
            if (mainCategoryId is null && subCategoryId is null)
            {
                return products;
            }
            else if (subCategoryId is not null && mainCategoryId is null)
            {
                return products.Where(p => p.SubCategoryId == subCategoryId);
            }
            else if (mainCategoryId is not null && subCategoryId is not null)
            {
                return products.Where(p => p.MainCategoryId == mainCategoryId && p.SubCategoryId == subCategoryId);
            }
            else
            {
                return products.Where(p => p.MainCategoryId == mainCategoryId);
            }
        }

        public static IQueryable<Product> FilteredBySearchTerm(this IQueryable<Product> products, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return products;
            }
            else
            {
                return products.Where(prd => prd.ProductName.ToLower()
                .Contains(searchTerm.ToLower()));
            }
        }

        public static IQueryable<Product> FilteredByPrice(this IQueryable<Product> products, int? minPrice, int? maxPrice, bool isValidPrice)
        {
            if (isValidPrice)
            {
                return products.Where(prd => prd.ActualPrice >= minPrice && prd.ActualPrice <= maxPrice);
            }
            else
            {
                return products;
            }
        }

        public static IQueryable<Product> FilteredByShowcase(this IQueryable<Product> products, bool? isShowCase)
        {
            if (isShowCase != null && isShowCase == true)
            {
                return products.Where(prd => prd.ShowCase == true);
            }
            else
            {
                return products;
            }
        }

        public static IQueryable<Product> FilteredByDiscount(this IQueryable<Product> products, bool? isDiscount)
        {
            if (isDiscount != null && isDiscount == true) 
            {
                return products.Where(prd => prd.DiscountPrice != null);
            }
            else
            {
                return products;
            }
        }

        public static IQueryable<Product> ToPaginate(this IQueryable<Product> products, int pageNumber, int pageSize)
        {
            return products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}