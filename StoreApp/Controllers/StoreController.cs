using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;

namespace StoreApp.Controllers
{
    public class StoreController : Controller
    {
        private ProductService productService;
        public StoreController()
        {
            productService = new ProductService();
        }
        // GET: Store
        public ActionResult Index()
        {
            var products = productService.GetAll();

            if (products == null)
            {
                ViewBag.Message = "No products";
                return View();
            }
            else
            {
                return View(products);
            }
        }
    }
}