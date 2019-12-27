using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Filter;
using StoreApp.Models.Store;
using StoreApp.Util;
using StoreApp.Enums;
using System.Xml;

namespace StoreApp.Controllers
{
    public class StoreController : Controller
    {
        private UnitService unitservice;
        private SaveProductImageService saveProductImage;
        private ProductService productService;
        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;
        private FilterProductsService filterProductsService;
        private readonly WebMapper webMapper;

        public StoreController()
        {
            unitservice = new UnitService();
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

            TempData["StatusMessage"] = StateMessage.success.ToString();
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

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Product edited successful";

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
                TempData["StatusMessage"] = StateMessage.danger.ToString();
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

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ProductViewModel productVM = webMapper.config.Map<ProductDTO, ProductViewModel>(productService.Get(id));

            if (productVM == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "The product wasn't added to the cart";
            }

            var cart = TempData["Сart"] as List<ProductViewModel> ?? new List<ProductViewModel>();

            ProductViewModel productCart = cart.FirstOrDefault(p => p.Id == id);

            if (productCart != null)
            {
                productCart.Quantity++;
            }
            else
            {
                productVM.Quantity = 1;
                cart.Add(productVM);
            }

            TempData["Сart"] = cart;
            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "The product was added to the cart";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadXML(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0 || file.ContentType != "text/xml")
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "File empty or isn't XML";
                return RedirectToAction("Index");
            }

            XmlDocument xmlFile = new XmlDocument();

            xmlFile.Load(file.InputStream);

            XmlElement products = xmlFile.DocumentElement;

            if (products.Name != "products")
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "File XML don't exist products root";
                return RedirectToAction("Index");
            }

            int countUpload = 0;
            int countNotUpload = 0;

            foreach (XmlElement productXML in products)
            {
                ProductDTO productDTO = webMapper.Map(productXML);


                if (productDTO != null && productService.ValidateNewProduct(productDTO))
                {
                    if (productDTO.Quantity.Equals(0))
                    {
                        productDTO.Enable = false;
                    }
                    productService.Create(productDTO);
                    countUpload++;
                }
                else
                {
                    countNotUpload++;
                }
            }

            TempData["StatusMessage"] = StateMessage.info.ToString();
            TempData["Message"] = $"{countUpload} product's upload successful and {countNotUpload} is not";

            return RedirectToAction("Index");
        }

        private ReferenceProducts GetReferences()
        {
            return new ReferenceProducts()
            {
                Units = new SelectList(unitservice.GetAll(), dataValueField: "Id", dataTextField: "Name"),
                Categories = new SelectList(categoryService.GetAll(), dataValueField: "Id", dataTextField: "Name"),
                Brands = new SelectList(brandService.GetAll(), dataValueField: "Id", dataTextField: "Name"),
                Producers = new SelectList(producerService.GetAll(), dataValueField: "Id", dataTextField: "Name")
            };
        }
    }
}