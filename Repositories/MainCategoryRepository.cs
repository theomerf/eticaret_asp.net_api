using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public sealed class MainCategoryRepository
    : RepositoryBase<MainCategory>, IMainCategoryRepository
    {
        public MainCategoryRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateCategory(MainCategory mainCategory)
        {
            Create(mainCategory);
        }

        public void DeleteOneCategory(MainCategory mainCategory)
        {
            Remove(mainCategory);
        }

        public void UpdateOneCategory(MainCategory mainCategory)
        {
            Update(mainCategory);
        }

        public async Task<IEnumerable<MainCategory>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Include(mc => mc.SubCategories).ToListAsync();
        }

        public async Task<MainCategory?> GetOneCategoryAsync(int id, bool trackChanges)
        {
            return await FindByCondition(p => p.MainCategoryId.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
    }
}
