using System.Linq;
using System.Web.Mvc;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Filter;
using StoreApp.Models.Store;
using StoreApp.Util;

namespace StoreApp.Controllers
{
    public class StoreController : Controller
    {
        private ProductService productService;
        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;
        private readonly WebMapper webMapper;

        public StoreController()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
            brandService = new BrandService();
            producerService = new ProducerService();
            webMapper  = new WebMapper();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var products = productService.GetAll().Select(p => webMapper.Map(p));
            
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

        [HttpPost]
        public ActionResult Index(FilterProducts filter)
        {
            var products = productService.GetAll().Select(p => webMapper.Map(p));

            TempData["Filter"] = filter;

            if (!ModelState.IsValid)
            {
                return View(products);
            }

            if (filter.Name != null)
            {
                products = products.Where(p => p.Name.Contains(filter.Name));
            }

            if (filter.PriceFrom > 0)
            {
                products = products.Where(p => p.Price >= filter.PriceFrom);
            }

            if (filter.PriceTo > 0)
            {
                products = products.Where(p => p.Price < filter.PriceTo);
            }

            if (filter.Enable == true)
            {
                products = products.Where(p => p.Enable == true);
            }

            

            if (filter.CategoryId > 0)
            {
                string category = categoryService.Get(filter.CategoryId ?? 0).Name;

                products = products.Where(p => p.Category.Equals(category));
            }

            if (filter.BrandId > 0)
            {
                string brand = brandService.Get(filter.BrandId ?? 0).Name;

                products = products.Where(p => p.Brand.Equals(brand));
            }

            if (filter.ProducerId > 0)
            {
                string producer = producerService.Get(filter.ProducerId ?? 0).Name;

                products = products.Where(p => p.Producer.Equals(producer));
            }

            return View(products);
        }

        public ActionResult FilterMenuPartial()
        {
            TempData["Categories"] = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Brands"] = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Producers"] = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name");

            return PartialView("_FilterMenuPartial", TempData["Filter"]);
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

            ProductDTO productDTO = webMapper.config.Map<CreateProductViewModel, ProductDTO>(product);

            productService.Create(productDTO);

            TempData["Message"] = "New product created seccessful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            EditProductViewModel product = webMapper.config.Map<ProductDTO, EditProductViewModel>(productService.Get(id));

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            TempData["Categories"] = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Brands"] = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Producers"] = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name");

            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(EditProductViewModel product)
        {
            TempData["Categories"] = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Brands"] = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name");
            TempData["Producers"] = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Product haven't edited");
                return View(product);
            }
            
            productService.Edit(webMapper.config.Map<EditProductViewModel, ProductDTO>(product));

            TempData["Message"] = "Product have edited";

            return View(product);
        }

        [HttpGet]
        public ActionResult DetailsProduct(int id)
        {
            ProductDTO productDTO = productService.Get(id);

            ProductViewModel product = webMapper.Map(productDTO);

            return View(product);
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            ProductDTO productDTO = productService.Get(id);

            ProductViewModel product = webMapper.Map(productDTO);

            return View(product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(ProductViewModel product)
        {
            if (product == null)
            {
                TempData["Message"] = "Product don't delete!";
                return RedirectToAction("Index");
            }

            productService.Delete(product.Id);

            return RedirectToAction("Index");
        }

    }
}