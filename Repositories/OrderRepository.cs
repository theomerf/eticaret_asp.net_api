using Entities.Models;
using Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Entities.RequestParameters;
using Repositories.Extensions;

namespace Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext context) : base(context)
        {

        }

        public IQueryable<Order> Orders => _context.Orders
        .Include(o => o.Lines)
        .OrderBy(o => o.Shipped)
        .ThenByDescending(o => o.Shipped);

        public int NumberOfInProcess => _context.Orders.Count(o => o.Shipped.Equals(false));

        public void Complete(int id)
        {
            var order = FindByCondition(O => O.OrderId.Equals(id), true);
            if(order is null)
            {
                throw new Exception("Order could not found");
            }
            order.Shipped = true;
        }

        public Order? GetOneOrder(int id)
        {
            return FindByCondition(o => o.OrderId.Equals(id), false);
        }

        public IQueryable<Order> GetUserOrders(string userName)
        {
            return _context
                .Orders
                .Where(o => o.UserName == userName)
                .Include(mc => mc.Lines);
        }

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l));
            if(order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}
