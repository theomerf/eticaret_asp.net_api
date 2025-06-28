using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IMainCategoryService
    {
        Task<IEnumerable<MainCategoryDto>> GetAllCategoriesAsync(bool trackChanges);
        Task CreateCategoryAsync(MainCategoryDtoForInsertion categoryDto);
        Task UpdateCategoryAsync(MainCategoryDtoForUpdate categoryDto);
        Task<MainCategoryDtoForUpdate> GetOneCategoryForUpdateAsync(int id, bool trackChanges);
        Task DeleteOneCategoryAsync(int id);
    }
}
