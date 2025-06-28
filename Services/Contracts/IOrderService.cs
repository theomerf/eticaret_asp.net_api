using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOneOrderAsync(int id);
        Task CompleteAsync(int id);
        Task SaveOrderAsync(OrderDto order);
        Task<int> GetNumberOfInProcessAsync();
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string? userName);
    }
}
