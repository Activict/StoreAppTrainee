using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    class OrderDetailRepository : IRepository<OrderDetail>
    {
        private StoreContext db;
        public OrderDetailRepository(StoreContext context)
        {
            db = context;
        }

        public void Create(OrderDetail item)
        {
            db.OrderDetails.Add(item);
        }

        public void Delete(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail != null)
            {
                db.OrderDetails.Remove(orderDetail);
            }
        }

        public void Detach(OrderDetail item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<OrderDetail> Find(Func<OrderDetail, bool> predicate)
        {
            return db.OrderDetails.Where(predicate).ToList();
        }

        public OrderDetail Get(int id)
        {
            return db.OrderDetails.Find(id);
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return db.OrderDetails.ToList();
        }

        public void Update(OrderDetail item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
