using StoreApp.DAL.Entities;
using System;

namespace StoreApp.DAL.Intefaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        IRepository<Order> Orders { get; }
        IRepository<User> Users { get; }
        IRepository<Category> Categories { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Producer> Producers { get; }
        IRepository<Unit> Units { get; } 

        void Save();
    }
    
}
