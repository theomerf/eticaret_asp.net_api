using Entities.Models;
using Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Entities.RequestParameters;
using Repositories.Extensions;

namespace Repositories
{
    public sealed class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync() => await FindAll(false)
            .Include(o => o.Lines)
            .OrderByDescending(o => o.Shipped)
            .ToListAsync();

        public async Task<int> GetNumberOfInProcessAsync() => await FindAllByCondition(o => o.Shipped.Equals(false), false).CountAsync();

        public async Task CompleteAsync(int id)
        {
            var order = await FindByCondition(O => O.OrderId.Equals(id), true).SingleOrDefaultAsync();
            if (order is null)
            {
                throw new Exception("Order could not found");
            }
            order.Shipped = true;
        }

        public async Task<Order?> GetOneOrderAsync(int id)
        {
            return await FindByCondition(o => o.OrderId.Equals(id), false).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string? userName)
        {
            return await FindAllByCondition(o => o.UserName == userName, false)
                .Include(mc => mc.Lines)
                .ToListAsync();
        }

        public async Task SaveOrderAsync(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l));
            if (order.OrderId == 0)
            {
                await _context.Orders.AddAsync(order);
            }
            await _context.SaveChangesAsync();
        }
    }
}
