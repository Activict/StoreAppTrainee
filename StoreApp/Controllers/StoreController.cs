using System.Web.Mvc;
using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Store;

namespace StoreApp.Controllers
{
    public class StoreController : Controller
    {
        private ProductService productService;

        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;

        private IMapper config;

        public StoreController()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
            brandService = new BrandService();
            producerService = new ProducerService();

            config = new MapperConfiguration(cfg => cfg.CreateMap<CreateProductViewModel, ProductDTO>()).CreateMapper();
        }

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
            TempData["Categories"] = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Brands"] = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Producers"] = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name");

            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(CreateProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "New product doesn't create");
                TempData["Categories"] = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name");
                TempData["Brands"] = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name");
                TempData["Producers"] = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name");
                return View(product);
            }

            ProductDTO productDTO = config.Map<CreateProductViewModel, ProductDTO>(product); 

            productService.Create(productDTO);

            TempData["Message"] = "New product created seccessful!";

            return RedirectToAction("Index");
        }
    }
}