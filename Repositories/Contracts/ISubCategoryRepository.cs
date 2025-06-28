using Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
    {
        Task<IEnumerable<SubCategory>> GetAllCategoriesAsync(bool trackChanges);
        void CreateCategory(SubCategory subCategory);
        void UpdateOneCategory(SubCategory subCategory);
        Task<SubCategory?> GetOneCategoryAsync(int id, bool trackChanges);
        void DeleteOneCategory(SubCategory subCategory);
    }
}
