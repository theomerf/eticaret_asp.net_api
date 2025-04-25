using Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserReviewRepository : IRepositoryBase<UserReview>
    {
        IQueryable<UserReview> GetAllUserReviews(bool trackChanges);
        IQueryable<UserReview> GetAllUserReviewsOfOneProduct(int id, bool trackChanges);
        IQueryable<UserReview> GetAllUserReviewsOfOneUser(string id, bool trackChanges);
        UserReview? GetOneUserReview(int id, bool trackChanges);
        void CreateUserReview(UserReview userReview);
        void DeleteOneUserReview(UserReview userReview);
        void UpdateOneUserReview(UserReview entity);
    }
}
