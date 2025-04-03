using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class MainCategoryRepository
    : RepositoryBase<MainCategory>, IMainCategoryRepository
    {
        public MainCategoryRepository(RepositoryContext context) : base(context)
        {

        }

    }
}
