using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models

{
    public class Order
    {
        public int OrderId { get; set; }
        public String? UserName{ get; set; }
        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();

        [Required(ErrorMessage ="Name is required.")]
        public String? Name { get; set; }

        [Required(ErrorMessage="Line1 is required.")]
        public String? Line1 { get; set; }
        public String? Line2 { get; set; }
        public String? Line3 { get; set; }
        public String? City { get; set; }
        public bool GiftWrap { get; set; }
        public bool Shipped { get; set; } = false;
        public DateTime? OrderedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderLine
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public String? ProductName { get; set; } = null;
        public decimal? DiscountPrice { get; set; } = null;
        public decimal? ActualPrice { get; set; } = null;
        public string? ImageUrl { get; set; } = null;
    }
}