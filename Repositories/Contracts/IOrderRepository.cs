using Entities.Models;

namespace Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOneOrderAsync(int id);
        Task CompleteAsync(int id);
        Task SaveOrderAsync(Order order);
        Task<int> GetNumberOfInProcessAsync();
        Task<IEnumerable<Order>> GetUserOrdersAsync(string? userName);
    }

}
