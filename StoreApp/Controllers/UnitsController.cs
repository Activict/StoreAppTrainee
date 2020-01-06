using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Enums;
using StoreApp.Models.Unit;
using StoreApp.Util;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StoreApp.Controllers
{
    [Authorize]
    public class UnitsController : Controller
    {
        private UnitService unitService;
        private IMapper config;

        public UnitsController()
        {
            unitService = new UnitService();

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UnitDTO, UnitViewModel>();
                cfg.CreateMap<UnitViewModel, UnitDTO>();
            }).CreateMapper();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var units = config.Map<IEnumerable<UnitDTO>, IEnumerable<UnitViewModel>>(unitService.GetAll());
            
            return View(units);
        }

        [HttpGet]
        public ActionResult CreateUnit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUnit(UnitViewModel unit)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Unit edit is wrong!");
                return View(unit);
            }

            if (unitService.IsExistUnit(config.Map<UnitViewModel, UnitDTO>(unit)))
            {
                ModelState.AddModelError("", "Such unit already exist!");
                return View(unit);
            }

            UnitDTO unitDTO = config.Map<UnitViewModel, UnitDTO>(unit);

            unitService.Create(unitDTO);

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Unit created success";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditUnit(int id)
        {
            UnitViewModel unit = config.Map<UnitDTO, UnitViewModel>(unitService.Get(id));

            if (unit == null)
            {
                RedirectToAction("Index");
            }

            return View(unit);
        }

        [HttpPost]
        public ActionResult EditUnit(UnitViewModel unit)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Unit edited wrong!");
                return View(unit);
            }

            if (unitService.IsExistUnit(config.Map<UnitViewModel, UnitDTO>(unit)))
            {
                ModelState.AddModelError("", "Such unit already exist!");
                return View(unit);
            }

            unitService.Edit(config.Map<UnitViewModel, UnitDTO>(unit));

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Unit edited success";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsUnit(int id)
        {
            UnitViewModel unit = config.Map<UnitDTO, UnitViewModel>(unitService.Get(id));

            if (unit == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This unit isn't exist";
                return RedirectToAction("Index");
            }

            return View(unit);
        }

        [HttpGet]
        public ActionResult DeleteUnit(int id)
        {
            UnitViewModel unit = config.Map<UnitDTO, UnitViewModel>(unitService.Get(id));

            if (unit == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This unit isn't exist";
                return RedirectToAction("Index");
            }

            ViewBag.CountProducts = unitService.GetCountProductsByUnitId(id);

            return View(unit);
        }

        [HttpPost]
        public ActionResult DeleteUnit(UnitViewModel unit)
        {
            if (unit == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This unit isn't exist";
                return RedirectToAction("Index");
            }

            unitService.Delete(unit.Id);

            TempData["StatusMessage"] = StateMessage.danger.ToString();
            TempData["Message"] = "Unit deleted success!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadXML(HttpPostedFileBase file)
        {
            var fileManager = new FileManager(file, RootNames.units);

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