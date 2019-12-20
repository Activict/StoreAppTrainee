using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace StoreApp.DAL.Repositories
{
    class CategoryRepository : IRepository<Category>
    {
        private StoreContext db;
        public CategoryRepository(StoreContext context)
        {
            db = context;
        }
        public void Create(Category item)
        {
            db.Categories.Add(item);
        }

        public void Delete(int id)
        {
            Category category = db.Categories.Find(id);
            if (category != null)
            {
                db.Categories.Remove(category);
            }
        }

        public void Detach(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories;
        }

        public void Update(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
