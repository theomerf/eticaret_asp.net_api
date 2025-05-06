namespace Repositories.Contracts{
    public interface IRepositoryManager{
        IProductRepository Product {get; }
        IMainCategoryRepository MainCategory {get; }
        ISubCategoryRepository SubCategory { get; }
        IOrderRepository Order { get; }
        IUserReviewRepository UserReview { get; }
        ICartRepository Cart { get; }
        void Save();
    }
}