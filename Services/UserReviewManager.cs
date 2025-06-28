using AutoMapper;
using Entities.Models;
using Entities.Dtos;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserReviewManager : IUserReviewService
    {
        private readonly IRepositoryManager _manager;

        private readonly IMapper _mapper;
        public UserReviewManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task CreateUserReviewAsync(UserReviewDtoForInsertion userReviewDto)
        {
            UserReview userReview = _mapper.Map<UserReview>(userReviewDto);
            var ratings = await GetAllRatingsForProductAsync(userReview.ProductId, false);
            var product = await _manager.Product.GetOneProductAsync(userReview.ProductId, false);
            var ratingsList = ratings?.ToList();
            if (ratingsList != null && product != null)
            {
                ratingsList.Add(userReview.Rating);
                product.AverageRating = ratingsList.Average();
                _manager.Product.UpdateOneProduct(product);
            }

            _manager.UserReview.CreateUserReview(userReview);
            await _manager.SaveAsync();
        }

        public async Task DeleteOneUserReviewAsync(int id)
        {
            var userReview = await GetOneUserReviewAsync(id, false);
            if (userReview != null)
            {
                var ratings = await GetAllRatingsForProductAsync(userReview.ProductId, false);
                var product = await _manager.Product.GetOneProductAsync(userReview.ProductId, false);
                var ratingsList = ratings?.ToList();
                if (ratingsList != null && product != null)
                {
                    ratingsList.Remove(userReview.Rating);
                    product.AverageRating = ratingsList.Average();
                    _manager.Product.UpdateOneProduct(product);
                }
                _manager.UserReview.DeleteOneUserReview(_mapper.Map<UserReview>(userReview));
                await _manager.SaveAsync();
            }
        }

        public async Task<IEnumerable<UserReviewDto>> GetAllUserReviewsAsync(bool trackChanges)
        {
            var reviews = await _manager.UserReview.GetAllUserReviewsAsync(trackChanges);
            return _mapper.Map<IEnumerable<UserReviewDto>>(reviews);
        }

        public async Task<IEnumerable<UserReviewDto>> GetAllUserReviewsOfOneProductAsync(int id, bool trackChanges)
        {
            var reviews = await _manager.UserReview.GetAllUserReviewsOfOneProductAsync(id, trackChanges);
            return _mapper.Map<IEnumerable<UserReviewDto>>(reviews);
        }

        public async Task<IEnumerable<UserReviewDto>> GetAllUserReviewsOfOneUserAsync(string? id, bool trackChanges)
        {
            if(id != null)
            {
                var reviews = await _manager.UserReview.GetAllUserReviewsOfOneUserAsync(id, trackChanges);
                return _mapper.Map<IEnumerable<UserReviewDto>>(reviews);
            }
            else
            {
                throw new ArgumentNullException(nameof(id), "User ID null olamaz.");
            }
        }

        public async Task<IEnumerable<int>> GetAllRatingsForProductAsync(int id, bool trackChanges)
        {
            return await _manager.UserReview.GetAllRatingsForProductAsync(id, trackChanges);
        }

        public async Task<UserReviewDto?> GetOneUserReviewAsync(int id, bool trackChanges)
        {
            var userReview = await _manager.UserReview.GetOneUserReviewAsync(id, trackChanges);
            if (userReview == null)
                throw new Exception($"{id} numaralı yorum yok.");
            return _mapper.Map<UserReviewDto>(userReview);
        }

        public async Task UpdateOneUserReviewAsync(UserReviewDtoForUpdate userReviewDto)
        {
            var userReview = _mapper.Map<UserReview>(userReviewDto);
            var ratings = await GetAllRatingsForProductAsync(userReview.ProductId, false);
            var product = await _manager.Product.GetOneProductAsync(userReview.ProductId, false);
            var ratingsList = ratings?.ToList();
            if (ratingsList != null && product != null)
            {
                ratingsList.Add(userReview.Rating);
                product.AverageRating = ratingsList.Average();
                _manager.Product.UpdateOneProduct(product);
            }
            _manager.UserReview.UpdateOneUserReview(_mapper.Map<UserReview>(userReview));
            await _manager.SaveAsync();
        }

        public async Task<int> GetCountAsync(bool trackChanges)
        {
            return await _manager.UserReview.CountAsync(trackChanges);
        }
    }
}
