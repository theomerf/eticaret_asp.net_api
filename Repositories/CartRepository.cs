using Entities.Models;
using Repositories.Contracts;
using System;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public sealed class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateCart(Cart cart)
        {
            Create(cart);
        }

        public void UpdateCart(Cart cart)
        {
            Update(cart);
        }

        public Cart GetCartByUserId(string userId)
        {
            return _context.Carts
                .Include(c => c.Lines)
                .FirstOrDefault(c => c.UserId == userId);
        }
    }
}
