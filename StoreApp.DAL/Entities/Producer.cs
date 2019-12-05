using System.Collections.Generic;

namespace StoreApp.DAL.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public Producer()
        {
            Products = new List<Product>();
        }
    }
}
