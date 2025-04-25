using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class UserReviewRepository : RepositoryBase<UserReview>, IUserReviewRepository
    {
        public UserReviewRepository(RepositoryContext context) : base(context)
        {
        }
        public void CreateUserReview(UserReview userReview)
        {
            Create(userReview);
        }
        public void DeleteOneUserReview(UserReview userReview) => Remove(userReview);
        public IQueryable<UserReview> GetAllUserReviews(bool trackChanges) => FindAll(trackChanges);
        public IQueryable<UserReview> GetAllUserReviewsOfOneProduct(int id, bool trackChanges)
        {
            return FindAll(trackChanges)
                .Where(p => p.ProductId.Equals(id));
        }
        public IQueryable<UserReview> GetAllUserReviewsOfOneUser(string id, bool trackChanges)
        {
            return FindAll(trackChanges)
                .Where(u => u.UserId.Equals(id))
                .Include(u => u.Product);
        }

        public UserReview? GetOneUserReview(int id, bool trackChanges)
        {
            return FindByCondition(p => p.UserReviewId.Equals(id), trackChanges);
        }
        public void UpdateOneUserReview(UserReview entity) => Update(entity);
    }
}
