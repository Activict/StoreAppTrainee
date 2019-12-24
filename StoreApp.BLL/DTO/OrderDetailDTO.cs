namespace StoreApp.BLL.DTO
{
    public class OrderDetailDTO
    {
        public OrderDetailDTO() {  }
        public OrderDetailDTO(int orderId, int productId, decimal price, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
