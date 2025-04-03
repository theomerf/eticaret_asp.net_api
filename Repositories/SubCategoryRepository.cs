using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class SubCategoryRepository
    : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(RepositoryContext context) : base(context)
        {

        }

    }
}
