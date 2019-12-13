using System.Linq;
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

            config = new MapperConfiguration(cfg => { cfg.CreateMap<CreateProductViewModel, ProductDTO>();
                                                      cfg.CreateMap<ProductDTO, EditProductViewModel>();
                                                      cfg.CreateMap<EditProductViewModel, ProductDTO>();
                                                      cfg.CreateMap<ProductDTO, DetailsProductViewModel>();
                                                    }).CreateMapper();
        }

        public ActionResult Index()
        {
            var products = productService.GetAll()
                .Select(p => new ProductViewModel()
                {   
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    Picture = p.Picture,
                    Quality = p.Quality,
                    Enable = p.Enable,
                    Category = categoryService.Get(p.CategoryId)?.Name,
                    Brand = brandService.Get(p.BrandId)?.Name,
                    Producer = producerService.Get(p.ProducerId)?.Name
                });

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

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            EditProductViewModel product = config.Map<ProductDTO, EditProductViewModel>(productService.Get(id));

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
            
            productService.Edit(config.Map<EditProductViewModel, ProductDTO>(product));

            TempData["Message"] = "Product have edited";

            return View(product);
        }

        [HttpGet]
        public ActionResult DetailsProduct(int id)
        {
            ProductDTO productDTO = productService.Get(id);

            DetailsProductViewModel product = new DetailsProductViewModel() { 
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                Unit = productDTO.Unit,
                Picture = productDTO.Picture,
                Quality = productDTO.Quality,
                Enable = productDTO.Enable,
                Category = categoryService.Get(productDTO.CategoryId)?.Name,
                Brand = brandService.Get(productDTO.BrandId)?.Name,
                Producer = producerService.Get(productDTO.ProducerId)?.Name
            };

            return View(product);
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            ProductDTO productDTO = productService.Get(id);

            DetailsProductViewModel product = new DetailsProductViewModel()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                Unit = productDTO.Unit,
                Picture = productDTO.Picture,
                Quality = productDTO.Quality,
                Enable = productDTO.Enable,
                Category = categoryService.Get(productDTO.CategoryId)?.Name,
                Brand = brandService.Get(productDTO.BrandId)?.Name,
                Producer = producerService.Get(productDTO.ProducerId)?.Name
            };

            return View(product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(DetailsProductViewModel product)
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