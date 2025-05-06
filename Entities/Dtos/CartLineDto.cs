using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CartLineDto
    {
        public int CartLineId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        public decimal ActualPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public CartDto Cart { get; set; } = new();
    }

}
