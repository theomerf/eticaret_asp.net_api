using Entities.Dtos;

namespace Entities.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string? UserId { get; set; }
        public List<CartLine> Lines { get; set; }
        public Cart()
        {
            Lines = new List<CartLine>();
        }

        public virtual void AddItem(ProductDto product, int quantity)
        {
            CartLine? line = Lines.Where(l => l.ProductId.Equals(product.ProductId)).FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "",
                    ImageUrl = product.ImageUrl ?? "",
                    ActualPrice = product.ActualPrice,
                    DiscountPrice = product.DiscountPrice ?? 0,
                    CartId = CartId,
                    Cart = this,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(int productId)
        {
            Lines.RemoveAll(l => l.ProductId.Equals(productId));
        }

        public virtual void IncreaseQuantity(int productId, int quantity)
        {
            CartLine? line = Lines.Where(l => l.ProductId.Equals(productId)).FirstOrDefault();

            if (line != null)
            {
                line.Quantity += quantity;
            }
        }

        public virtual void DecreaseQuantity(int productId, int quantity)
        {
            CartLine? line = Lines.Where(l => l.ProductId.Equals(productId)).FirstOrDefault();

            if (line != null)
            {
                line.Quantity -= quantity;
            }
        }

        public decimal ComputeTotalValue() =>
            Lines.Sum((Func<CartLine, decimal>)(e => (decimal)(e.ActualPrice * e.Quantity)));

        public decimal ComputeTotalDiscountValue() =>
            Lines.Sum((Func<CartLine, decimal>)(e => (decimal)(e.DiscountPrice ?? 0 * e.Quantity)));

        public virtual void Clear() => Lines.Clear();
    }
}
