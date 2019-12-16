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

        [HttpGet]
        public ActionResult EditProducer(int id)
        {
            ProducerViewModel producer = config.Map<ProducerDTO, ProducerViewModel>(producerService.Get(id));

            if (producer == null)
            {
                RedirectToAction("Index");
            }

            return View(producer);
        }

        [HttpPost]
        public ActionResult EditProducer(ProducerViewModel producer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Producer edited wrong!");
                return View(producer);
            }

            producerService.Edit(config.Map<ProducerViewModel, ProducerDTO>(producer));

            TempData["Message"] = "Producer edited success!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsProducer(int id)
        {
            ProducerViewModel producer = config.Map<ProducerDTO, ProducerViewModel>(producerService.Get(id));

            if (producer == null)
            {
                TempData["Message"] = "This producer isn't exist";
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        [HttpGet]
        public ActionResult DeleteProducer(int id)
        {
            ProducerViewModel producer = config.Map<ProducerDTO, ProducerViewModel>(producerService.Get(id));

            if (producer == null)
            {
                TempData["Message"] = "This producer isn't exist";
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        [HttpPost]
        public ActionResult DeleteProducer(ProducerViewModel producer)
        {
            if (producer == null)
            {
                TempData["Message"] = "This producer isn't exist";
                return RedirectToAction("Index");
            }

            producerService.Delete(producer.Id);

            TempData["Message"] = "Producer deleted success!";

            return RedirectToAction("Index");
        }

    }
}