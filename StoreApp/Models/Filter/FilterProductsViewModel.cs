namespace StoreApp.Models.Filter
{
    public class FilterProductsViewModel
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