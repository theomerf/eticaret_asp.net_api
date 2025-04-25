using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserReviewService
    {
        IEnumerable<UserReview> GetAllUserReviews(bool trackChanges);
        IEnumerable<UserReview> GetAllUserReviewsOfOneProduct(int id, bool trackChanges);
        IEnumerable<UserReview> GetAllUserReviewsOfOneUser(string id, bool trackChanges);
        UserReview? GetOneUserReview(int id, bool trackChanges);
        void CreateUserReview(UserReviewDtoForInsertion userReview);
        void DeleteOneUserReview(int id);
        void UpdateOneUserReview(UserReviewDtoForUpdate userReview);
    }
}
