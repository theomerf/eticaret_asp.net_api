using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IMainCategoryService
    {
        IEnumerable<MainCategory> GetAllCategories(bool trackChanges);
        void CreateCategory(MainCategoryDtoForInsertion categoryDto);
        void UpdateCategory(MainCategoryDtoForUpdate categoryDto);
        MainCategoryDtoForUpdate GetOneCategoryForUpdate(int id, bool trackChanges);
        void DeleteOneCategory(int id);
    }
}
