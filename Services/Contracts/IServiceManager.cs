namespace Services.Contracts
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IMainCategoryService MainCategoryService { get; }
        ISubCategoryService SubCategoryService { get; }
        IOrderService OrderService { get; }
        IAuthService AuthService { get; }
        IUserReviewService UserReviewService { get; }
    }
}
