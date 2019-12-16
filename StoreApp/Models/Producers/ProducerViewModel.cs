using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Producers
{
    public class ProducerViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}