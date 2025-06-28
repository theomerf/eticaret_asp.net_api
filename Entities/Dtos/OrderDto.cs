using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record OrderDto
    {
        public int OrderId { get; set; }
        public String? UserName { get; set; }
        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();

        [Required(ErrorMessage = "Name is required.")]
        public String? Name { get; set; }

        [Required(ErrorMessage = "Line1 is required.")]
        public String? Line1 { get; set; }
        public String? Line2 { get; set; }
        public String? Line3 { get; set; }
        public String? City { get; set; }
        public bool Shipped { get; set; } = false;
        public bool GiftWrap { get; set; }
        public DateTime? OrderedAt { get; set; } = DateTime.UtcNow;
    }
}
