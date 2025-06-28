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

        private readonly IUserReviewRepository _userReviewRepository;
        private readonly ICartRepository _cartRepository;


        public RepositoryManager(IProductRepository productRepository, RepositoryContext context, IMainCategoryRepository mainCategoryRepository, IOrderRepository orderRepository, ISubCategoryRepository subCategoryRepository, IUserReviewRepository userReviewRepository, ICartRepository cartRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _mainCategoryRepository = mainCategoryRepository;
            _orderRepository = orderRepository;
            _subCategoryRepository = subCategoryRepository;
            _userReviewRepository = userReviewRepository;
            _cartRepository = cartRepository;
        }

        public IProductRepository Product => _productRepository;

        public IMainCategoryRepository MainCategory => _mainCategoryRepository;

        public IOrderRepository Order => _orderRepository;

        public ISubCategoryRepository SubCategory => _subCategoryRepository;

        public IUserReviewRepository UserReview => _userReviewRepository;
        public ICartRepository Cart => _cartRepository;

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}