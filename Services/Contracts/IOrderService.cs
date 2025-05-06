using Entities.Models;

namespace Services.Contracts
{
    public interface IOrderService
    {
        IEnumerable<Order> Orders { get; }
        Order? GetOneOrder(int id);
        void Complete(int id);
        void SaveOrder(Order order);
        int NumberOfInProcess { get; }
        IQueryable<Order> GetUserOrders(string userName);
    }
}
