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

                ViewBag.TotalPriceCart = cart.Sum(p => p.Price * p.Quantity);
                ViewBag.CountToCart = cart.Sum(p => p.Quantity);
            }

            return PartialView("_CartPartial");
        }

        [HttpGet]
        public ActionResult DeleteFromCart(int id)
        {
            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                var product = cart.FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    cart.Remove(product);
                    TempData["Cart"] = cart;
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

                cart.FirstOrDefault(p => p.Id == id).Quantity++;

                TempData["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DecreaseProduct(int id)
        {
            if (TempData.Peek("Сart") is List<ProductViewModel>)
            {
                var cart = TempData.Peek("Сart") as List<ProductViewModel>;

                var product = cart.FirstOrDefault(p => p.Id == id && p.Quantity == 1);

                if (product != null)
                {
                    cart.Remove(product);
                }
                else
                {
                    cart.FirstOrDefault(p => p.Id == id && p.Quantity > 1).Quantity--;
                }

                TempData["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Order()
        {
            var cart = TempData.Peek("Сart") as List<ProductViewModel>;

            if (cart == null || !cart.Any())
            {
                TempData["StatusMessage"] = "danger";
                TempData["Message"] = "The order wasn't created. Cart is empty";
                return RedirectToAction("Index", "Store");
            }

            var userId = userService.GetAll().First(u => u.UserName.Equals(User.Identity.Name))?.Id;

            if (!userId.HasValue)
            {
                TempData["StatusMessage"] = "danger";
                TempData["Message"] = $"The user wasn't found by name '{User.Identity.Name}'";
                return RedirectToAction("Index", "Store");
            }

            var orderId = orderService.Create(userId.Value, cart.Sum(p => p.Price * p.Quantity));

            cart.ForEach(p => orderDetailService.Create(new OrderDetailDTO(orderId, p.Id, p.Price, p.Quantity)));

            TempData["Сart"] = null;
            TempData["StatusMessage"] = "success";
            TempData["Message"] = "Order was created successful";

            return RedirectToAction("Index");
        }
    }
}