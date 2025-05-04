using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using System.Text;

namespace ETicaret.Infrastructure.Extensions
{
    public class ImportFromCsvExtension
    {
        private readonly RepositoryContext _context;

        public ImportFromCsvExtension(RepositoryContext context)
        {
            _context = context;
        }

        public void ImportProductsFromCsv(string csvFilePath)
        {
            if(_context.Products.Any())
            {
                return; 
            }
            var products = new List<Product>();
            var mainCategories = new List<MainCategory>();
            var subCategories = new List<SubCategory>();
            var lines = File.ReadAllLines(csvFilePath, Encoding.UTF8);

            for (int i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split(';');

                var product = new Product()
                {
                    ProductName = columns[1].Trim(),
                    MainCategoryId = int.TryParse(columns[2], out var mainCategoryId1) ? mainCategoryId1 : 0,
                    SubCategoryId = int.TryParse(columns[4], out var subCategoryId3) ? subCategoryId3 : 0,
                    ImageUrl = columns[6].Trim(),
                    ActualPrice = decimal.TryParse(columns[8], out var actualPrice) ? actualPrice : 0,
                    DiscountPrice = decimal.TryParse(columns[7], out var discountPrice) ? discountPrice : 0,
                    Summary = columns[9].Trim(),
                    ShowCase = columns[10] == "true" ? true : false,
                };

                products.Add(product);

                var mainCategory = new MainCategory()
                {
                    CategoryName = columns[3].Trim()
                };

                if (i != 1 && !mainCategories.Any(c => c.CategoryName == columns[3]))
                {
                    mainCategories.Add(mainCategory);
                }

                var subCategory = new SubCategory()
                {
                    CategoryName = columns[5].Trim(),
                    MainCategoryId = int.TryParse(columns[2], out var mainCategoryId) ? mainCategoryId : 0,
                };

                if (i != 1 && !subCategories.Any(c => c.CategoryName == columns[5]))
                {
                    subCategories.Add(subCategory);
                }

            }

            _context.MainCategories.AddRange(mainCategories);
            _context.SaveChanges();
            _context.SubCategories.AddRange(subCategories);
            _context.SaveChanges();
            _context.Products.AddRange(products);
            _context.SaveChanges();
        }
    }
}
