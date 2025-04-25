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

        public void CreateUserReview(UserReviewDtoForInsertion userReviewDto)
        {
            UserReview userReview = _mapper.Map<UserReview>(userReviewDto);
            _manager.UserReview.CreateUserReview(userReview);
            _manager.Save();
        }

        public void DeleteOneUserReview(int id)
        {
            var userReview = GetOneUserReview(id, false);
            if (userReview != null)
            {
                _manager.UserReview.DeleteOneUserReview(userReview);
                _manager.Save();
            }
        }

        public IEnumerable<UserReview> GetAllUserReviews(bool trackChanges)
        {
            return _manager.UserReview.GetAllUserReviews(trackChanges);
        }

        public IEnumerable<UserReview> GetAllUserReviewsOfOneProduct(int id, bool trackChanges)
        {
            return _manager.UserReview.GetAllUserReviewsOfOneProduct(id, trackChanges);
        }

        public IEnumerable<UserReview> GetAllUserReviewsOfOneUser(string id, bool trackChanges)
        {
            return _manager.UserReview.GetAllUserReviewsOfOneUser(id, trackChanges);
        }

        public UserReview? GetOneUserReview(int id, bool trackChanges)
        {
            var userReview = _manager.UserReview.GetOneUserReview(id, trackChanges);
            if (userReview == null)
                throw new Exception($"{id} numaralı yorum yok.");
            return userReview;
        }

        public void UpdateOneUserReview(UserReviewDtoForUpdate userReviewDto)
        {
            var userReview = GetOneUserReview(userReviewDto.UserReviewId, true);
            _mapper.Map(userReviewDto, userReview);
            _manager.UserReview.UpdateOneUserReview(userReview);
            _manager.Save();
        }
    }
}
