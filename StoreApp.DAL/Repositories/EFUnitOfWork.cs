using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;

namespace StoreApp.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private StoreContext db;
        private ProductRepository productRepository;
        private OrderRepository orderRepository;
        private OrderDetailRepository orderDetailRepository;
        private UserRepository userRepository;
        private CategoryRepository categoryRepository;
        private BrandRepository brandRepository;
        private ProducerRepository producerRepository;
        private UnitRepository unitRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new StoreContext(connectionString);
        }

        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(db);
                }
                return productRepository;
            }
        }
        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new OrderRepository(db);
                }
                return orderRepository;
            }
        }
        public IRepository<OrderDetail> OrderDetails
        {
            get
            {
                if (orderDetailRepository == null)
                {
                    orderDetailRepository = new OrderDetailRepository(db);
                }
                return orderDetailRepository;
            }
        }
        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(db);
                }
                return userRepository;
            }
        }
        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new CategoryRepository(db);
                }
                return categoryRepository;
            }
        }
        public IRepository<Brand> Brands
        {
            get
            {
                if (brandRepository == null)
                {
                    brandRepository = new BrandRepository(db);
                }
                return brandRepository;
            }
        }
        public IRepository<Producer> Producers
        {
            get
            {
                if (producerRepository == null)
                {
                    producerRepository = new ProducerRepository(db);
                }
                return producerRepository;
            }
        }
        public IRepository<Unit> Units
        {
            get
            {
                if (unitRepository == null)
                {
                    unitRepository = new UnitRepository(db);
                }
                return unitRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
