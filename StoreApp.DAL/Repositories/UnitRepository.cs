using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreApp.DAL.Repositories
{
    class UnitRepository : IRepository<Unit>
    {
        private StoreContext db;
        public UnitRepository(StoreContext context)
        {
            db = context;
        }
        public void Create(Unit item)
        {
            db.Units.Add(item);
        }

        public void Delete(int id)
        {
            Unit unit = db.Units.Find(id);
            if (unit != null)
            {
                db.Units.Remove(unit);
            }
        }

        public IEnumerable<Unit> Find(Func<Unit, bool> predicate)
        {
            return db.Units.Where(predicate).ToList();
        }

        public Unit Get(int id)
        {
            return db.Units.Find(id);
        }

        public IEnumerable<Unit> GetAll()
        {
            return db.Units.ToList();
        }

        public void Update(Unit item)
        {
           db.Entry(item).State = EntityState.Modified;
        }

        public void Detach(Unit item)
        {
            db.Entry(item).State = EntityState.Detached;
        }
    }
}
