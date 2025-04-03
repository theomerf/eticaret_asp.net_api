using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public int MainCategoryId { get; set; }
        public MainCategory MainCategory { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
