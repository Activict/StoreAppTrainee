using StoreApp.DAL.Entities;
using System.Collections.Generic;

namespace StoreApp.BLL.DTO
{
    public class CategoryDTO
    {
        public CategoryDTO( Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
