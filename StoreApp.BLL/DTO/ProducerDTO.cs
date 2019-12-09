﻿using StoreApp.DAL.Entities;
using System.Collections.Generic;

namespace StoreApp.BLL.DTO
{
    public class ProducerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Producer> Producers { get; set; }
    }
}
