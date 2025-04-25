using Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
    {
        IQueryable<SubCategory> GetAllCategories(bool trackChanges);
        void CreateCategory(SubCategory subCategory);
        void UpdateOneCategory(SubCategory subCategory);
        SubCategory? GetOneCategory(int id, bool trackChanges);
        void DeleteOneCategory(SubCategory subCategory);
    }
}
