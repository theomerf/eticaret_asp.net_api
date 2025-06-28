using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
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

        public async Task CreateProductAsync(ProductDtoForInsertion productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            _manager.Product.CreateProduct(product);
            await _manager.SaveAsync();
        }

        public async Task DeleteOneProductAsync(int id)
        {
            var product = await GetOneProductForServiceAsync(id, false);
            _manager.Product.DeleteOneProduct(product);
            await _manager.SaveAsync();

        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool trackChanges)
        {
            var products = await _manager.Product.GetAllProductsAsync(trackChanges);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductWithRatingDto>> GetFavouriteProductsAsync(UserDtoForFavourites favouritesDto, bool trackChanges)
        {
            var products = await _manager.Product.GetFavouriteProductsAsync(favouritesDto.FavouriteProductsId, trackChanges);
            return _mapper.Map<IEnumerable<ProductWithRatingDto>>(products);
        }

        public async Task<int> GetCountAsync(bool trackChanges)
        {
            return await _manager.Product.GetCountAsync(trackChanges);
        }

        public async Task<int> GetAllProductsCountWithDetailsAsync(ProductRequestParameters p)
        {
            var productsCount = await _manager.Product.GetAllProductsCountWithDetailsAsync(p);
            return productsCount;
        }

        public async Task<IEnumerable<ProductWithRatingDto>> GetAllProductsWithDetailsAsync(ProductRequestParameters p)
        {
            var products = await _manager.Product.GetAllProductsWithDetailsAsync(p);
            var productDto = _mapper.Map<IEnumerable<ProductWithRatingDto>>(products);
            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsWithDetailsAdminAsync(ProductRequestParameters p)
        {
            var products = await _manager.Product.GetAllProductsWithDetailsAdminAsync(p);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetLastestProductsAsync(int n, bool trackChanges)
        {
            var products = await _manager.Product.GetLastestProductsAsync(n, trackChanges);  
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetOneProductAsync(int id, bool trackChanges)
        {
            var product = await _manager.Product.GetOneProductAsync(id, trackChanges);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<Product> GetOneProductForServiceAsync(int id, bool trackChanges)
        {
            var product = await _manager.Product.GetOneProductAsync(id, trackChanges);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }
            return product;
        }

        public async Task<ProductDtoForUpdate> GetOneProductForUpdateAsync(int id, bool trackChanges)
        {
            var product = await GetOneProductForServiceAsync(id, trackChanges);
            var productDto = _mapper.Map<ProductDtoForUpdate>(product);
            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetShowcaseProductsAsync(bool trackChanges)
        {
            var products = await _manager.Product.GetShowcaseProductsAsync(trackChanges);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductWithRatingDto>> GetShowcaseProductsWithRatingsAsync(bool trackChanges)
        {
            var products = await _manager.Product.GetShowcaseProductsWithRatingsAsync(trackChanges);
            return _mapper.Map<IEnumerable<ProductWithRatingDto>>(products);
        }

        public async Task UpdateOneProductAsync(ProductDtoForUpdate productDto)
        {
            var entity = _mapper.Map<Product>(productDto);
            _manager.Product.UpdateOneProduct(entity);
            await _manager.SaveAsync();
        }

    }
}
