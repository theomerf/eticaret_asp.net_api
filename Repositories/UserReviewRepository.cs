using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public sealed class UserReviewRepository : RepositoryBase<UserReview>, IUserReviewRepository
    {
        public UserReviewRepository(RepositoryContext context) : base(context)
        {
        }
        public void CreateUserReview(UserReview userReview)
        {
            Create(userReview);
        }
        public void DeleteOneUserReview(UserReview userReview) => Remove(userReview);
        public void UpdateOneUserReview(UserReview entity) => Update(entity);
        public async Task<int> CountAsync(bool trackChanges)
        {
            return await Count(trackChanges);
        }
        public async Task<IEnumerable<UserReview>> GetAllUserReviewsAsync(bool trackChanges) => await FindAll(trackChanges).ToListAsync();
        public async Task<IEnumerable<UserReview>> GetAllUserReviewsOfOneProductAsync(int id, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Where(p => p.ProductId.Equals(id))
                .ToListAsync();
        }
        public async Task<IEnumerable<int>> GetAllRatingsForProductAsync(int id, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Where(p => p.ProductId.Equals(id))
                .Select(p => p.Rating)
                .ToListAsync();
        }
        public async Task<IEnumerable<UserReview>> GetAllUserReviewsOfOneUserAsync(string id, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .Where(u => u.UserId != null && u.UserId.Equals(id))
                .Include(u => u.Product)
                .ToListAsync();
        }

        public async Task<UserReview?> GetOneUserReviewAsync(int id, bool trackChanges)
        {
            return await FindByCondition(p => p.UserReviewId.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
    }
}
