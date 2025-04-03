using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class MainCategoryManager : IMainCategoryService
    {
        private readonly IRepositoryManager _manager;

        public MainCategoryManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<MainCategory> GetAllCategories(bool trackChanges)
        {
            return _manager.MainCategory.FindAll(trackChanges);
        }
    }
}
