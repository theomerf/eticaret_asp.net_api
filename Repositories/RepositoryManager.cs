using Repositories.Contracts;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;

        private readonly IProductRepository _productRepository;

        private readonly IMainCategoryRepository _mainCategoryRepository;
        
        private readonly IOrderRepository _orderRepository;

        private readonly ISubCategoryRepository _subCategoryRepository;


        public RepositoryManager(IProductRepository productRepository, RepositoryContext context, IMainCategoryRepository mainCategoryRepository, IOrderRepository orderRepository, ISubCategoryRepository subCategoryRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _mainCategoryRepository = mainCategoryRepository;
            _orderRepository = orderRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        public IProductRepository Product => _productRepository;

        public IMainCategoryRepository MainCategory => _mainCategoryRepository;

        public IOrderRepository Order => _orderRepository;

        public ISubCategoryRepository SubCategory => _subCategoryRepository;

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}