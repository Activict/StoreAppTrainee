using System.Linq;
using System.Web.Mvc;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
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
            webMapper = new WebMapper();
        }

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
            ProductDTO productDTO = webMapper.config.Map<CreateProductViewModel, ProductDTO>(product);

            bool validate = productService.ValidateNewProduct(productDTO);

            if (!ModelState.IsValid || !validate)
            {
                if (!validate)
                {
                    ModelState.AddModelError("", "Such product already exist");
                }
                else
                {
                    ModelState.AddModelError("", "New product doesn't create");
                }

                TempData["Categories"] = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name");
                TempData["Brands"] = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name");
                TempData["Producers"] = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name");
                return View(product);
            }

            if (productDTO.Quantity.Equals(0))
            {
                productDTO.Enable = false;
            }

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

            ProductDTO productDTO = webMapper.config.Map<EditProductViewModel, ProductDTO>(product);

            bool validate = productService.ValidateEditProduct(productDTO);

            if (!ModelState.IsValid || !validate)
            {
                if (!validate)
                {
                    ModelState.AddModelError("", "Such product already exist");
                }
                else
                {
                    ModelState.AddModelError("", "Product haven't edited");
                }
                
                return View(product);
            }

            if (productDTO.Quantity.Equals(0))
            {
                productDTO.Enable = false;
            }

            productService.Edit(productDTO);

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