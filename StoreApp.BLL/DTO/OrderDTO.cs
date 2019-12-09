using System;

namespace StoreApp.BLL.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public int Discount { get; set; }
        public DateTime OrderTime { get; set; }

        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
