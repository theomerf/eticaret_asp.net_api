using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderManager : IOrderService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public OrderManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync() => _mapper.Map<IEnumerable<OrderDto>>(await _manager.Order.GetAllOrdersAsync());

        public async Task<int> GetNumberOfInProcessAsync() => await _manager.Order.GetNumberOfInProcessAsync();

        public async Task CompleteAsync(int id)
        {
            await _manager.Order.CompleteAsync(id);
            await _manager.SaveAsync();
        }

        public async Task<OrderDto?> GetOneOrderAsync(int id)
        {
            var orders = await _manager.Order.GetOneOrderAsync(id);
            return _mapper.Map<OrderDto>(orders);
        }

        public async Task SaveOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _manager.Order.SaveOrderAsync(order);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string? userName)
        {
            var orders = await _manager.Order.GetUserOrdersAsync(userName);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}
