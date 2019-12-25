using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    class ProducerRepository : IRepository<Producer>
    {
        private StoreContext db;
        public ProducerRepository(StoreContext context)
        {
            db = context;
        }

        public void Create(Producer item)
        {
            db.Producers.Add(item);
        }

        public void Delete(int id)
        {
            Producer producer = db.Producers.Find(id);
            if (producer != null)
            {
                db.Producers.Remove(producer);
            }
        }

        public void Detach(Producer item)
        {
            db.Entry(item).State = EntityState.Detached;
        }

        public IEnumerable<Producer> Find(Func<Producer, bool> predicate)
        {
            return db.Producers.Where(predicate).ToList();
        }

        public Producer Get(int id)
        {
            return db.Producers.Find(id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return db.Producers.ToList();
        }

        public void Update(Producer item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
