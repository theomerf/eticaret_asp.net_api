using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record OrderDtoForCreation: OrderDto
    {
        public ICollection<CartLineDto> CartLines { get; set; } = new List<CartLineDto>();
        public CartDto? Cart { get; set; } = null;
    }
}
