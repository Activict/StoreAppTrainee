﻿using System.Collections.Generic;

namespace StoreApp.DAL.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public Brand()
        {
            Products = new List<Product>();
        }
    }
}
