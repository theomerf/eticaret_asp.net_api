using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class SubCategoryManager : ISubCategoryService
    {
        private readonly IRepositoryManager _manager;

        public SubCategoryManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<SubCategory> GetAllCategories(bool trackChanges)
        {
            return _manager.SubCategory.FindAll(trackChanges);
        }
    }
}
