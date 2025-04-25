using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class SubCategoryRepository
    : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateCategory(SubCategory subCategory)
        {
            Create(subCategory);
        }

        public void DeleteOneCategory(SubCategory subCategory)
        {
            Remove(subCategory);
        }

        public IQueryable<SubCategory> GetAllCategories(bool trackChanges)
        {
            return FindAll(trackChanges);
        }

        public SubCategory? GetOneCategory(int id, bool trackChanges)
        {
            return FindByCondition(p => p.SubCategoryId.Equals(id), trackChanges);
        }

        public void UpdateOneCategory(SubCategory subCategory)
        {
            Update(subCategory);
        }

    }
}
