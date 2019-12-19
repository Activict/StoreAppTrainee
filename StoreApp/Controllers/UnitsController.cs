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

            TempData["Meaasage"] = "Unit created success";

            return RedirectToAction("Index");
        }


    }
}