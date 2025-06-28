using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public sealed class SubCategoryRepository
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

        public void UpdateOneCategory(SubCategory subCategory)
        {
            Update(subCategory);
        }

        public async Task<IEnumerable<SubCategory>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

        public async Task<SubCategory?> GetOneCategoryAsync(int id, bool trackChanges)
        {
            return await FindByCondition(p => p.SubCategoryId.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

    }
}
