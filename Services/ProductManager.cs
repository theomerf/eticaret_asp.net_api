using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ProductManager : IProductService
    {
        private readonly IRepositoryManager _manager;

        private readonly IMapper _mapper;

        public ProductManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void CreateProduct(ProductDtoForInsertion productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            _manager.Product.CreateProduct(product);
            _manager.Save();
        }

        public void DeleteOneProduct(int id)
        {
            var product = GetOneProduct(id, false);
            if (product != null)
            {
                _manager.Product.DeleteOneProduct(product);
                _manager.Save();
            }
        }

        public IEnumerable<Product> GetAllProducts(bool trackChanges)
        {
            var products = _manager.Product.GetAllProducts(trackChanges).ToList();
            return products;
        }

        public IEnumerable<ProductWithRatingDto> GetFavouriteProducts(ICollection<int> favouriteProductIds, bool trackChanges)
        {
            var products = _manager.Product.GetFavouriteProducts(favouriteProductIds, trackChanges).ToList();
            return products;
        }

        public int GetCount(bool trackChanges)
        {
            return _manager.Product.GetCount(trackChanges);
        }

        public IEnumerable<ProductWithRatingDto> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            var products = _manager.Product.GetAllProductsWithDetails(p).ToList();
            return products;
        }

        public IEnumerable<Product> GetAllProductsWithDetailsAdmin(ProductRequestParameters p)
        {
            var products = _manager.Product.GetAllProductsWithDetailsAdmin(p).ToList();
            return products;
        }

        public IEnumerable<Product> GetLastestProducts(int n, bool trackChanges)
        {
            return _manager.
                Product
                .FindAll(trackChanges)
                .OrderByDescending(prd => prd.ProductId)
                .Take(n)
                .ToList();
        }

        public Product? GetOneProduct(int id, bool trackChanges)
        {
            var product = _manager.Product.GetOneProduct(id, trackChanges);
            if (product == null)
            {
                throw new Exception("Ürün bulunamadı");
            }
            return product;
        }

        public ProductDtoForUpdate GetOneProductForUpdate(int id, bool trackChanges)
        {
            var product = GetOneProduct(id, trackChanges);
            var productDto = _mapper.Map<ProductDtoForUpdate>(product);
            return productDto;
        }

        public IEnumerable<Product> GetShowcaseProducts(bool trackChanges)
        {
            var products = _manager.Product.GetShowcaseProducts(trackChanges).ToList();
            return products;
        }

        public IEnumerable<ProductWithRatingDto> GetShowcaseProductsWithRatings(bool trackChanges)
        {
            var products = _manager.Product.GetShowcaseProductsWithRatings(trackChanges).ToList();
            return products;
        }

        public void UpdateOneProduct(ProductDtoForUpdate productDto)
        {
            var entity = _mapper.Map<Product>(productDto);
            _manager.Product.UpdateOneProduct(entity);
            _manager.Save();
        }

    }
}
