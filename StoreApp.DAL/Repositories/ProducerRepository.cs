using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;

namespace StoreApp.DAL.Repositories
{
    class ProducerRepository : IRepository<Producer>
    {
        private StoreContext db;
        public ProducerRepository(StoreContext context)
        {
            this.db = context;
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

        public IEnumerable<Producer> Find(Func<Producer, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Producer Get(int id)
        {
            return db.Producers.Find(id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return db.Producers;
        }

        public void Update(Producer item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
