using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Brands;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class BrandsController : Controller
    {
        private BrandService brandService;
        private IMapper config;

        public BrandsController()
        {
            brandService = new BrandService();
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandDTO, BrandViewModel>();
                cfg.CreateMap<BrandViewModel, BrandDTO>();
            }).CreateMapper();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var brands = config.Map<IEnumerable<BrandDTO>, IEnumerable<BrandViewModel>>(brandService.GetAll());
            
            return View(brands);
        }

        [HttpGet]
        public ActionResult CreateBrand()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateBrand(BrandViewModel brand)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Brand create is wrong!");
                return View(brand);
            }

            BrandDTO brandDTO = config.Map<BrandViewModel, BrandDTO>(brand);

            brandService.Create(brandDTO);

            TempData["Message"] = "Brand created success!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsBrand(int id)
        {
            BrandViewModel brand = config.Map<BrandDTO, BrandViewModel>(brandService.Get(id));

            if (brand == null)
            {
                TempData["Message"] = "This brand isn't exist";
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        [HttpGet]
        public ActionResult EditBrand(int id)
        {
            BrandViewModel brand = config.Map<BrandDTO, BrandViewModel>(brandService.Get(id));

            if (brand == null)
            {
                TempData["Message"] = "This brand isn't exist";
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        [HttpPost]
        public ActionResult EditBrand(BrandViewModel brand)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Brand edited wrong!");
                return View(brand);
            }

            brandService.Edit(config.Map<BrandViewModel, BrandDTO>(brand));

            TempData["Message"] = "Brand edited success!";

            return RedirectToAction("Index");
        }
    }
}