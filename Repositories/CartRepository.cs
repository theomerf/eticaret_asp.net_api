using Entities.Models;
using Repositories.Contracts;
using System;
using Microsoft.EntityFrameworkCore;
using Entities.Dtos;

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

        public async Task<Cart?> GetCartByUserIdAsync(string? userId)
        {
            return await FindByCondition(c => c.UserId == userId, false)
                .OfType<Cart>()
                .Include(c => c.Lines)
                .SingleOrDefaultAsync();
        }
    }
}
