using Entities.Models;

namespace Repositories.Contracts
{
    public interface IMainCategoryRepository : IRepositoryBase<MainCategory>
    {
        Task<IEnumerable<MainCategory>> GetAllCategoriesAsync(bool trackChanges);
        void CreateCategory(MainCategory mainCategory);
        void UpdateOneCategory(MainCategory mainCategory);
        Task<MainCategory?> GetOneCategoryAsync(int id, bool trackChanges);
        void DeleteOneCategory(MainCategory mainCategory);
    }
}
