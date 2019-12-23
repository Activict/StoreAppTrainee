using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
        private SaveProductImageService saveProductImage;
        private ProductService productService;
        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;
        private FilterProductsService filterProductsService;
        private readonly WebMapper webMapper;

        public StoreController()
        {
            saveProductImage = new SaveProductImageService();
            productService = new ProductService();
            categoryService = new CategoryService();
            brandService = new BrandService();
            producerService = new ProducerService();
            filterProductsService = new FilterProductsService();
            webMapper = new WebMapper();
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
        public ActionResult Index(FilterProductsViewModel filter)
        {
            TempData["Filter"] = filter;

            if (!ModelState.IsValid)
            {
                var products = productService.GetAll().Select(p => webMapper.Map(p));

                return View(products);
            }

            FilterProductsDTO filterDTO = webMapper.config.Map<FilterProductsViewModel, FilterProductsDTO>(filter);

            var productsFiltered = filterProductsService.GetProductsDTOFiltered(filterDTO).Select(p => webMapper.Map(p));

            return View(productsFiltered);
        }

        public ActionResult FilterMenuPartial()
        {
            ViewBag.References = GetReferences();

            return PartialView("_FilterMenuPartial", TempData["Filter"]);
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            ViewBag.References = GetReferences();

            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(CreateProductViewModel product, HttpPostedFileBase file)
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

                ViewBag.References = GetReferences();
                return View(product);
            }

            if (productDTO.Quantity.Equals(0))
            {
                productDTO.Enable = false;
            }

            if (file != null && file.ContentLength > 0)
            {
                bool isEnableImageExtension = ConfigurationManager.AppSettings.Get("imageExtension").Contains(file.ContentType.ToLower());
                
                if (!isEnableImageExtension)
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension");
                    ViewBag.References = GetReferences();
                    return View(product);
                }

                productDTO.Picture = file.FileName;

                productService.Create(productDTO);
                saveProductImage.SaveImage(productDTO, file);
            }
            else
            {
                productService.Create(productDTO);
            }

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

            ViewBag.References = GetReferences();

            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(EditProductViewModel product, HttpPostedFileBase file)
        {
            ViewBag.References = GetReferences();

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

            if (file != null && file.ContentLength > 0)
            {
                bool isEnableImageExtension = ConfigurationManager.AppSettings.Get("imageExtension").Contains(file.ContentType.ToLower());

                if (!isEnableImageExtension)
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension");
                    ViewBag.References = GetReferences();
                    return View(product);
                }

                productDTO.Picture = file.FileName;

                productService.Edit(productDTO);
                saveProductImage.SaveImage(productDTO, file);
            }
            else
            {
                productService.Edit(productDTO);
            }

            product = webMapper.config.Map<ProductDTO, EditProductViewModel>(productDTO);

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

            var pathStringPictures = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Pictures\\Products"));
            var pathStringProductsById = Path.Combine(pathStringPictures.ToString(), product.Id.ToString());

            if (Directory.Exists(pathStringProductsById))
            {
                Directory.Delete(pathStringProductsById, true);
            }

            productService.Delete(product.Id);

            return RedirectToAction("Index");
        }

        private ReferenceProducts GetReferences()
        {
            return new ReferenceProducts()
            {
                Categories = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name"),
                Brands = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name"),
                Producers = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name")
            };
        }
    }
}