using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Store
{
    public class CreateProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Picture { get; set; }
        public string Quality { get; set; }
        public bool Enable { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [DisplayName("Brand")]
        public int BrandId { get; set; }
        [DisplayName("Producer")]
        public int ProducerId { get; set; }
    }
}