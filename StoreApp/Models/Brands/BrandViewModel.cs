using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Brands
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}