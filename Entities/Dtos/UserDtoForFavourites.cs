using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record UserDtoForFavourites
    {
        public ICollection<int> FavouriteProductsId { get; set; } = new List<int>();
    }
}
