using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface ISubCategoryService
    {
        IEnumerable<SubCategory> GetAllCategories(bool trackChanges);
        void CreateCategory(SubCategoryDtoForInsertion categoryDto);
        void UpdateCategory(SubCategoryDtoForUpdate categoryDto);
        SubCategoryDtoForUpdate GetOneCategoryForUpdate(int id, bool trackChanges);
        void DeleteOneCategory(int id);
    }
}
