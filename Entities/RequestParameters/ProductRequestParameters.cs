using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entities.RequestParameters
{
    public class ProductRequestParameters : RequestParameters
    {
        public int? MainCategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; } = int.MaxValue;
        public bool IsValidPrice => MaxPrice > MinPrice;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Brand { get; set; }
        public bool? IsShowCase { get; set; } = null;
        public bool? IsDiscount { get; set; } = null;
        public ProductRequestParameters() : this(1, 6)
        {
        }
        public ProductRequestParameters(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}