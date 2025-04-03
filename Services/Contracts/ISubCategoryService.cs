using Entities.Models;

namespace Services.Contracts
{
    public interface ISubCategoryService
    {
        IEnumerable<SubCategory> GetAllCategories(bool trackChanges);
    }
}
