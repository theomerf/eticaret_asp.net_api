using Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserReviewRepository : IRepositoryBase<UserReview>
    {
        Task<IEnumerable<UserReview>> GetAllUserReviewsAsync(bool trackChanges);
        Task<int> CountAsync(bool trackChanges);
        Task<IEnumerable<UserReview>> GetAllUserReviewsOfOneProductAsync(int id, bool trackChanges);
        Task<IEnumerable<UserReview>> GetAllUserReviewsOfOneUserAsync(string id, bool trackChanges);
        Task<UserReview?> GetOneUserReviewAsync(int id, bool trackChanges);
        void CreateUserReview(UserReview userReview);
        void DeleteOneUserReview(UserReview userReview);
        void UpdateOneUserReview(UserReview entity);
        Task<IEnumerable<int>> GetAllRatingsForProductAsync(int id, bool trackChanges);
    }
}
