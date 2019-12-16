using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}