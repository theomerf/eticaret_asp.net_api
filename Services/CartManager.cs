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

namespace Services
{
    public class CartManager : ICartService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public CartManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void AddOrUpdateCart(CartDto cartDto, string userId)
        {
            if (!cartDto.Lines.Any())
            {
                var existingCart = _manager.Cart.GetCartByUserId(userId);
                if (existingCart == null || !existingCart.Lines.Any())
                {
                    return;
                }
            }

            var cartDb = _manager.Cart.GetCartByUserId(userId);
            Cart cart = _mapper.Map<Cart>(cartDto);
            bool dbUpdate = false;

            if (cartDb == null)
            {
                _manager.Cart.CreateCart(cart);
                _manager.Save();
            }
            else
            {
                // Eğer gelen sepet boşsa, mevcut tüm satırları temizle
                if (!cart.Lines.Any() && cartDb.Lines.Any())
                {
                    cartDb.Lines.Clear();
                    dbUpdate = true;
                }
                else
                {
                    // cartDto'da olmayanları sil
                    var linesToRemove = cartDb.Lines
                        .Where(dbLine => !cart.Lines.Any(dtoLine => dtoLine.ProductId == dbLine.ProductId))
                        .ToList();

                    foreach (var lineToRemove in linesToRemove)
                    {
                        cartDb.Lines.Remove(lineToRemove);
                        dbUpdate = true;
                    }

                    // Güncelle veya yeni satır ekle
                    foreach (var lineDto in cart.Lines)
                    {
                        var line = cartDb.Lines.FirstOrDefault(l => l.ProductId == lineDto.ProductId);

                        if (line != null)
                        {
                            if (line.Quantity != lineDto.Quantity)
                            {
                                line.Quantity = lineDto.Quantity;
                                dbUpdate = true;
                            }
                        }
                        else
                        {
                            cartDb.Lines.Add(new CartLine
                            {
                                ProductId = lineDto.ProductId,
                                Quantity = lineDto.Quantity,
                                ActualPrice = lineDto.ActualPrice,
                                DiscountPrice = lineDto.DiscountPrice,
                                ProductName = lineDto.ProductName,
                                ImageUrl = lineDto.ImageUrl
                            });
                            dbUpdate = true;
                        }
                    }
                }

                if (dbUpdate)
                {
                    _manager.Cart.UpdateCart(cartDb);
                    _manager.Save();
                }
            }
        }

    }
}
