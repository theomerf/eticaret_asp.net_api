namespace Entities.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; }
        public Cart()
        {
            Lines = new List<CartLine>();
        }

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = Lines.Where(l => l.Product.ProductId.Equals(product.ProductId)).FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
        {
            Lines.RemoveAll(l => l.Product.ProductId.Equals(product.ProductId));
        }

        public virtual void IncreaseQuantity(Product product, int quantity)
        {
            CartLine? line = Lines.Where(l => l.Product.ProductId.Equals(product.ProductId)).FirstOrDefault();

            if (line != null)
            {
                line.Quantity += quantity;
            }
        }

        public virtual void DecreaseQuantity(Product product, int quantity)
        {
            CartLine? line = Lines.Where(l => l.Product.ProductId.Equals(product.ProductId)).FirstOrDefault();

            if (line != null)
            {
                line.Quantity -= quantity;
            }
        }

        public decimal ComputeTotalValue() =>
            Lines.Sum((Func<CartLine, decimal>)(e => (decimal)(e.Product.ActualPrice * e.Quantity)));

        public decimal ComputeTotalDiscountValue() =>
            Lines.Sum((Func<CartLine, decimal>)(e => (decimal)(e.Product.DiscountPrice * e.Quantity)));

        public virtual void Clear() => Lines.Clear();
    }
}
