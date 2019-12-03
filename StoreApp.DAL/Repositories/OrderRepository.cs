using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    class OrderRepository : IRepository<Order>
    {
        private StoreContext db;
        public OrderRepository(StoreContext context)
        {
            this.db = context;
        }

        public void Create(Order item)
        {
            db.Order.Add(item);
        }

        public void Delete(int id)
        {
            Order order = db.Order.Find(id);
            if (order != null)
            {
                db.Order.Remove(order);
            }
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return db.Order.Include("User").Where(predicate).ToList();
        }

        public Order Get(int id)
        {
            return db.Order.Find(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Order.Include("User");
        }

        public void Update(Order item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
