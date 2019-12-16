using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Producers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class ProducersController : Controller
    {
        private ProducerService producerService;
        private IMapper config;

        public ProducersController()
        {
            producerService = new ProducerService();

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProducerDTO, ProducerViewModel>();
                cfg.CreateMap<ProducerViewModel, ProducerDTO>();
            }).CreateMapper();
        }

        public ActionResult Index()
        {
            var producers = config.Map<IEnumerable<ProducerDTO>, IEnumerable<ProducerViewModel>>(producerService.GetAll());

            return View(producers);
        }

        [HttpGet]
        public ActionResult CreateProducer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProducer(ProducerViewModel producer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Producer edit is wrong");
                return View(producer);
            }

            ProducerDTO producerDTO = config.Map<ProducerViewModel, ProducerDTO>(producer);

            producerService.Create(producerDTO);

            TempData["Message"] = "Producer created success!";

            return RedirectToAction("Index");
        }
    }
}