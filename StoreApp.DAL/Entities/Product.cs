using System.Collections.Generic;

namespace StoreApp.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Picture { get; set; }
        public string Quality { get; set; }
        public bool Enable { get; set; }

        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int ProducerId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Producer Producer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
