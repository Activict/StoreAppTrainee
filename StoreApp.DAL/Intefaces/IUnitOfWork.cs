using StoreApp.DAL.Entities;
using System;

namespace StoreApp.DAL.Intefaces
{
    interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        IRepository<Order> Orders { get; }
        IRepository<User> Users { get; }

        void Save();
    }
    
}
