using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using ETicaret.Infrastructure.Extensions;
using System.Text.Json.Serialization;

namespace ETicaret.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;

            SessionCart cart  = session?.GetJson<SessionCart>("cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

         public static CartDto GetCartDto(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.GetJson<SessionCart>("cart") ?? new SessionCart();
            cart.Session = session;
            CartDto cartDto = new CartDto()
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Lines = cart.Lines.Select(l => new CartLineDto
                {
                    ProductId = l.ProductId,
                    ProductName = l.ProductName,
                    ImageUrl = l.ImageUrl,
                    ActualPrice = l.ActualPrice,
                    DiscountPrice = l.DiscountPrice,
                    Quantity = l.Quantity
                }).ToList()
            };

            return cartDto;
        }


        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session?.SetJson<SessionCart>("cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("cart");
        }

        public override void IncreaseQuantity(int productId, int quantity)
        {
            base.IncreaseQuantity(productId, quantity);
            Session?.SetJson<SessionCart>("cart", this);
        }

        public override void DecreaseQuantity(int productId, int quantity)
        {
            base.DecreaseQuantity(productId, quantity);
            Session?.SetJson<SessionCart>("cart", this);
        }

        public override void RemoveLine(int productId)
        {
            base.RemoveLine(productId);
            Session?.SetJson<SessionCart>("cart", this);
        }

    }
}
