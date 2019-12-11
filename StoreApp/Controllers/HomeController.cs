using StoreApp.DAL.EF;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Repositories;
using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;

namespace StoreApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //using (var db = new StoreContext())
            //{
            //    var list = db.Products.Include(p => p.Category).ToList();
            //    //db.Products.Include(x => x.Category).to
            //}

            //EFUnitOfWork eof = new EFUnitOfWork("DefaultConnection");
            
            //// Products
            //var product = new Product() { Name = "User1", BrandId = 2, CategoryId = 2, ProducerId = 2 };
            //eof.Products.Create(product); // good
            //eof.Save();
            //eof.Products.Delete(1);  // good
            //eof.Save();
            //Product prod = eof.Products.Get(2); // good
            //prod.Name = "change name";
            //eof.Products.Update(prod);  // good
            //eof.Save();

            //var colProduct = eof.Products.Find( (x) => { return x.Name == "cola" && x.Price > 10; } );

            //var getAll = eof.Products.GetAll(); // good
            //foreach (var item in getAll)
            //{
            //    var _cat = item.Category;
            //    var brend = item.Brand;
            //    var producer = item.Producer;
            //}

            //// User
            //var user = new User() { Email = "fromContr@", Password = "pass" };
            //eof.Users.Create(user); // good
            //eof.Save();
            //eof.Users.Delete(1);  // good
            //eof.Save();
            //User user1 = eof.Users.Get(2); // good
            //user1.Email = "change_email@";
            //eof.Users.Update(user1);  // good
            //eof.Save();

            //var getAlluser = eof.Users.GetAll(); // good
            //foreach (var item in getAlluser)
            //{
            //    var us = item.Email;
            //}

            //// Orders
            //var order = new Order() { UserId = 3, TotalCost = 22, OrderDate = DateTime.Now };
            //eof.Orders.Create(order); // good
            //eof.Save();
            //eof.Orders.Delete(1);  // good
            //eof.Save();
            //Order order1 = eof.Orders.Get(2); // good
            //order1.TotalCost = 1234;
            //eof.Orders.Update(order1);  // good
            //eof.Save();

            //var getAllorder = eof.Orders.GetAll(); // good
            //foreach (var item in getAllorder)
            //{
            //    var us = item.User;
            //}

            //// OrdersDetails
            //var orderDet = new OrderDetail() { ProductId = 3, OrderId = 4 };
            //eof.OrderDetails.Create(orderDet); // good
            //eof.Save();
            //eof.OrderDetails.Delete(1);  // good
            //eof.Save();
            //OrderDetail orderDet1 = eof.OrderDetails.Get(2); // good
            //orderDet1.Price = 1232455;
            //eof.OrderDetails.Update(orderDet1);  // good
            //eof.Save();

            //var getAllorderDet = eof.OrderDetails.GetAll(); // good
            //foreach (var item in getAllorderDet)
            //{
            //    var us = item;
            //}

            //// Category
            //var cat = new Category() { Name = "cat3" };
            //eof.Categories.Create(cat); // good
            //eof.Save();
            //eof.Categories.Delete(1);  // good
            //eof.Save();
            //Category cat1 = eof.Categories.Get(2); // good
            //cat1.Name = "change cat";
            //eof.Categories.Update(cat1);  // good
            //eof.Save();

            //var getAllcat = eof.Categories.GetAll(); // good
            //foreach (var item in getAllcat)
            //{
            //    var us = item;
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}