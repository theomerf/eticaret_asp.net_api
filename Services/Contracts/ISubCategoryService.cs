using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryDto>> GetAllCategoriesAsync(bool trackChanges);
        Task CreateCategoryAsync(SubCategoryDtoForInsertion categoryDto);
        Task UpdateCategoryAsync(SubCategoryDtoForUpdate categoryDto);
        Task<SubCategoryDtoForUpdate> GetOneCategoryForUpdateAsync(int id, bool trackChanges);
        Task DeleteOneCategoryAsync(int id);
    }
}
