using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Store;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class CartController : Controller
    {
        private OrderService orderService;
        private UserService userService;
        private OrderDetailService orderDetailService;

        public CartController()
        {
            orderService = new OrderService();
            userService = new UserService();
            orderDetailService = new OrderDetailService();
        }
        
        public ActionResult Index()
        {
            var products = TempData.Peek("Сart") as List<ProductViewModel> ?? new List<ProductViewModel>();

            return View(products);
        }

        public ActionResult CartPartial()
        {
            ViewBag.CountToCart = 0;
            ViewBag.TotalPriceCart = 0m;

            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                foreach (var product in cart)
                {
                    ViewBag.CountToCart += product.Quantity;
                    ViewBag.TotalPriceCart += product.Quantity * product.Price;
                }
            }

            return PartialView("_CartPartial");
        }

        [HttpGet]
        public ActionResult DeleteFromCart(int id)
        {
            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Id == id)
                    {
                        cart.RemoveAt(i);

                        TempData["Cart"] = cart;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult IncreaseProduct(int id)
        {
            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Id == id)
                    {
                        cart[i].Quantity++;

                        TempData["Cart"] = cart;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DecreaseProduct(int id)
        {
            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Id == id)
                    {
                        if (cart[i].Quantity > 1)
                        {
                            cart[i].Quantity--;
                        }
                        else
                        {
                            cart.RemoveAt(i);
                        }
                        
                        TempData["Cart"] = cart;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Order()
        {
            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                decimal totalPriceCart = 0;

                foreach (var product in cart)
                {
                    totalPriceCart += product.Quantity * product.Price;
                }

                int orderId = -1;

                if (cart.Count > 0 && totalPriceCart > 0)
                {
                    string username;
                    if (User.Identity.Name == "") username = "user1"; 
                    else username = User.Identity.Name;

                    int userId = userService.GetAll().First(u => u.UserName.Equals(username)).Id;

                    orderId = orderService.Create(userId, totalPriceCart);
                }
                else
                {
                    TempData["Message"] = "Order don't created. Cart is empty";

                    return RedirectToAction("Index", "Store");
                }

                foreach (var product in cart)
                {
                    OrderDetailDTO orderDetailDTO = new OrderDetailDTO()
                    {
                        OrderId = orderId,
                        Price = product.Price,
                        ProductId = product.Id,
                        Quantity = product.Quantity
                    };

                    orderDetailService.Create(orderDetailDTO);
                }

                TempData["Сart"] = null;
                TempData["Message"] = "Order created success";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Order don't created. Cart is empty";

                return RedirectToAction("Index", "Store");
            }
        }
    }
}