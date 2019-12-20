using StoreApp.Models.Store;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class CartController : Controller
    {
        
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
    }
}