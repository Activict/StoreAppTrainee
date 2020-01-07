using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Enums;
using StoreApp.Models.Brands;
using StoreApp.Util;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    [Authorize]
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

            if (brandService.IsExistBrand(config.Map<BrandViewModel, BrandDTO>(brand)))
            {
                ModelState.AddModelError("", "Such category already exist!");
                return View(brand);
            }

            BrandDTO brandDTO = config.Map<BrandViewModel, BrandDTO>(brand);

            brandService.Create(brandDTO);

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Brand created successful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsBrand(int id)
        {
            BrandViewModel brand = config.Map<BrandDTO, BrandViewModel>(brandService.Get(id));

            if (brand == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
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
                TempData["StatusMessage"] = StateMessage.danger.ToString();
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

            if (brandService.IsExistBrand(config.Map<BrandViewModel, BrandDTO>(brand)))
            {
                ModelState.AddModelError("", "Such brand already exist!");
                return View(brand);
            }

            brandService.Edit(config.Map<BrandViewModel, BrandDTO>(brand));

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Brand edited successful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteBrand(int id)
        {
            BrandViewModel brand = config.Map<BrandDTO, BrandViewModel>(brandService.Get(id));

            if (brand == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This brand isn't exist";
                return RedirectToAction("Index");
            }

            ViewBag.CountProducts = brandService.GetCountProductsByBrandId(id);

            return View(brand);
        }

        [HttpPost]
        public ActionResult DeleteBrand(BrandViewModel brand)
        {
            if (brand == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This brand isn't exist";
                return RedirectToAction("Index");
            }

            brandService.Delete(brand.Id);

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Brand deleted successful!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var fileManager = new FileManager(file, RootNames.brands);

            if (!fileManager.IsValidateFile())
            {
                TempData["StatusMessage"] = fileManager.StatusMessage;
                TempData["Message"] = fileManager.Message;
                return RedirectToAction("Index");
            }

            if (!fileManager.IsValidateRequirements())
            {
                TempData["StatusMessage"] = fileManager.StatusMessage;
                TempData["Message"] = fileManager.Message;
                return RedirectToAction("Index");
            }

            fileManager.SaveData();
            fileManager.SaveFile();

            TempData["StatusMessage"] = StateMessage.info.ToString();
            TempData["Message"] = fileManager.Message;

            return RedirectToAction("Index");
        }
    }
}