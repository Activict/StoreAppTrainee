using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class CartController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CartPartial()
        {
            ViewBag.CountToCart = 0;
            ViewBag.TotalPriceCart = 0m;

            if (Session["Сart"] is List<ProductDTO>)
            {
                var cart = Session["Сart"] as List<ProductDTO>;
                foreach (var product in cart)
                {
                    ViewBag.CountToCart += product.Quantity;
                    ViewBag.TotalPriceCart += product.Quantity * product.Price;
                }
            }

            return PartialView("_CartPartial");
        }
    }
}