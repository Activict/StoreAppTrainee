using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.BLL.DTO
{
    public class FilterProductsDTO
    {
        public string Name { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public bool Enable { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? ProducerId { get; set; }
    }
}
