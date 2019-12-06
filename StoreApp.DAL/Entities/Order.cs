using System;
using System.Collections.Generic;

namespace StoreApp.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public int Discount { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}
