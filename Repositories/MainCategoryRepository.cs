using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class MainCategoryRepository
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

        public IQueryable<MainCategory> GetAllCategories(bool trackChanges)
        {
            return FindAll(trackChanges).Include(mc => mc.SubCategories);
        }

        public MainCategory? GetOneCategory(int id, bool trackChanges)
        {
            return FindByCondition(p => p.MainCategoryId.Equals(id), trackChanges);
        }

        public void UpdateOneCategory(MainCategory mainCategory)
        {
            Update(mainCategory);
        }
    }
}
