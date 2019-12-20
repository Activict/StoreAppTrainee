using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private StoreContext db;
        public ProductRepository(StoreContext context)
        {
            db = context;
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
            return db.Products.Where(predicate).ToList();
        }

        public Product Get(int id)
        {
            // Explicit Loading 
            // Reference
            var getProd = db.Products.Find(id);
            db.Entry(getProd).Reference("Category").Load();
            db.Entry(getProd).Reference("Brand").Load();
            db.Entry(getProd).Reference("Producer").Load();
            // Collection
            db.Entry(getProd).Collection("OrderDetails").Load();
            return getProd;
            // Lazy loading
            //return db.Products.Find(id);
        }

        public IEnumerable<Product> GetAll()
        {
            // Lazy loading
            var getAll = db.Products.ToList();      

            //Eager loading
            //var getAll = db.Products.Include(p => p.Category).ToList();
            return getAll;
        }

        public void Update(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Detach(Product item)
        {
            db.Entry(item).State = EntityState.Detached;
        }
    }
}
