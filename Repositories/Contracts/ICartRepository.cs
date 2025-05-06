using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ICartRepository : IRepositoryBase<Cart>
    {
        void CreateCart(Cart cart);
        void UpdateCart(Cart cart);
        Cart GetCartByUserId(string userId);
    }
}
