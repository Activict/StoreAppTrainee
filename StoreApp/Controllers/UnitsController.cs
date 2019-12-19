using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Unit;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
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

            UnitDTO unitDTO = config.Map<UnitViewModel, UnitDTO>(unit);

            unitService.Create(unitDTO);

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

            unitService.Edit(config.Map<UnitViewModel, UnitDTO>(unit));

            TempData["Message"] = "Unit edited success";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsUnit(int id)
        {
            UnitViewModel unit = config.Map<UnitDTO, UnitViewModel>(unitService.Get(id));

            if (unit == null)
            {
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
                TempData["Message"] = "This unit isn't exist";
                return RedirectToAction("Index");
            }

            return View(unit);
        }

        [HttpPost]
        public ActionResult DeleteUnit(UnitViewModel unit)
        {
            if (unit == null)
            {
                TempData["Message"] = "This unit isn't exist";
                return RedirectToAction("Index");
            }

            unitService.Delete(unit.Id);

            TempData["Message"] = "Unit deleted success!";

            return RedirectToAction("Index");
        }
    }
}