using System.Data.Entity;
using StoreApp.DAL.Entities;

namespace StoreApp.DAL.EF
{
    public class StoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Producer> Producers { get; set; }

        static StoreContext()
        {
            Database.SetInitializer<StoreContext>(new StoreDbInitializer());
        }
        public StoreContext(string connectionString): base(connectionString)
        {  }
        public StoreContext() : base("DefaultConnection")
        {  }
    }

    public class StoreDbInitializer : CreateDatabaseIfNotExists<StoreContext>
    {
        protected override void Seed(StoreContext db)
        {
        } 
    }
}
