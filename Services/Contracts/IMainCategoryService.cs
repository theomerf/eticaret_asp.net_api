using Entities.Models;

namespace Services.Contracts
{
    public interface IMainCategoryService
    {
        IEnumerable<MainCategory> GetAllCategories(bool trackChanges);
    }
}
