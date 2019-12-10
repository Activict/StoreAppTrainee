using System.Collections.Generic;

namespace StoreApp.BLL.DTO
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BrandDTO> Brands { get; set; }
    }
}
