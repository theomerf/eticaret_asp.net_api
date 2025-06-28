﻿using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; } = new();
    }
}
