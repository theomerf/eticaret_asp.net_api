using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserReviewService
    {
        Task<IEnumerable<UserReviewDto>> GetAllUserReviewsAsync(bool trackChanges);
        Task<int> GetCountAsync(bool trackChanges);
        Task<IEnumerable<UserReviewDto>> GetAllUserReviewsOfOneProductAsync(int id, bool trackChanges);
        Task<IEnumerable<UserReviewDto>> GetAllUserReviewsOfOneUserAsync(string? id, bool trackChanges);
        Task<UserReviewDto?> GetOneUserReviewAsync(int id, bool trackChanges);
        Task CreateUserReviewAsync(UserReviewDtoForInsertion userReview);
        Task DeleteOneUserReviewAsync(int id);
        Task UpdateOneUserReviewAsync(UserReviewDtoForUpdate userReview);
        Task<IEnumerable<int>> GetAllRatingsForProductAsync(int id, bool trackChanges);
    }
}
