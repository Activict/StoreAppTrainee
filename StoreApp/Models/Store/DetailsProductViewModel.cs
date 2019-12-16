namespace StoreApp.Models.Store
{
    public class DetailsProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Picture { get; set; }
        public string Quality { get; set; }
        public bool Enable { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Producer { get; set; }
    }
}