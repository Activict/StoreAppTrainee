using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Unit
{
    public class UnitViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}