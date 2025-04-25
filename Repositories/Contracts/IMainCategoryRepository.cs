using Entities.Models;

namespace Repositories.Contracts
{
    public interface IMainCategoryRepository : IRepositoryBase<MainCategory>
    {
        IQueryable<MainCategory> GetAllCategories(bool trackChanges);
        void CreateCategory(MainCategory mainCategory);
        void UpdateOneCategory(MainCategory mainCategory);
        MainCategory? GetOneCategory(int id, bool trackChanges);
        void DeleteOneCategory(MainCategory mainCategory);
    }
}
