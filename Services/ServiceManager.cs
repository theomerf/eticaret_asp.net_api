using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IProductService _productService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;

        public ServiceManager(IProductService productService, IMainCategoryService mainCategoryService, IOrderService orderService, IAuthService authService, ISubCategoryService subCategoryService)
        {
            _productService = productService;
            _mainCategoryService = mainCategoryService;
            _orderService = orderService;
            _authService = authService;
            _subCategoryService = subCategoryService;
        }

        public IProductService ProductService => _productService;
        public IMainCategoryService MainCategoryService => _mainCategoryService;
        public ISubCategoryService SubCategoryService => _subCategoryService;
        public IOrderService OrderService => _orderService;
        public IAuthService AuthService => _authService;
    }
}
