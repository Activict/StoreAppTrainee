using StoreApp.Models.OrderDetails;
using System;
using System.Collections.Generic;

namespace StoreApp.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public int Discount { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual IEnumerable<OrderDetailsViewModel> OrderDetails { get; set; }
    }
}