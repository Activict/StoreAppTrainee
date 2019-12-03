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

        static StoreContext()
        {
            Database.SetInitializer<StoreContext>(new StoreDbInitializer());
        }
        public StoreContext(string connectionString): base(connectionString)
        {  }   
    }

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext db)
        {
            db.Products.Add(new Product { Name = "TestProduct",  Quantity = 1 });
            db.SaveChanges();
        } 
    }
}
