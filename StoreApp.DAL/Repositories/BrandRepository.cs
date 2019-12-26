﻿using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    class BrandRepository : IRepository<Brand>
    {
        private StoreContext db;
        public BrandRepository(StoreContext context)
        {
            db = context;
        }

        public void Create(Brand item)
        {
            db.Brands.Add(item);
        }

        public void Delete(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand != null)
            {
                db.Brands.Remove(brand);
            }
        }

        public void Detach(Brand item)
        {
            db.Entry(item).State = EntityState.Detached;
        }

        public IEnumerable<Brand> Find(Func<Brand, bool> predicate)
        {
            return db.Brands.Where(predicate).ToList();
        }

        public Brand Get(int id)
        {
            return db.Brands.Find(id);
        }

        public IEnumerable<Brand> GetAll()
        {
            return db.Brands.ToList();
        }

        public void Update(Brand item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
