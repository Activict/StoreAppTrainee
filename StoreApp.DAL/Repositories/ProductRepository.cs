using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private StoreContext db;
        public ProductRepository(StoreContext context)
        {
            this.db = context;
        }

        public void Create(Product item)
        {
            db.Products.Add(item);
        }

        public void Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
            }
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return db.Products.Include("Category")
                              .Include("Brand")
                              .Include("Producer")
                              .Where(predicate)
                              .ToList();
        }

        public Product Get(int id)
        {
            return db.Products.Find(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.Include("Category")
                              .Include("Brand")
                              .Include("Producer");
        }

        public void Update(Product item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
