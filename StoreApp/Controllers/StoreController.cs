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

        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;

        public StoreController()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
            brandService = new BrandService();
            producerService = new ProducerService();
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

        [HttpGet]
        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "New product doesn't create");
                return View(product);
            }
            if (product== null)
            {
                return View();
            }

            productService.Create(product);

            ViewBag.Categories = new SelectList(categoryService.GetAll());
            ViewBag.Brands = new SelectList(brandService.GetAll());
            ViewBag.Producers = new SelectList(producerService.GetAll());

            TempData["Message"] = "New product created seccessful!";

            return View(product);
        }
    }
}