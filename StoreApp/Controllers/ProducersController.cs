using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Enums;
using StoreApp.Models.Producers;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StoreApp.Controllers
{
    [Authorize]
    public class ProducersController : Controller
    {
        private SaveXMLService saveXMLService;
        private ProducerService producerService;
        private IMapper config;

        public ProducersController()
        {
            saveXMLService = new SaveXMLService();
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

            if (producerService.IsExistProducer(config.Map<ProducerViewModel, ProducerDTO>(producer)))
            {
                ModelState.AddModelError("", "Such producer already exist!");
                return View(producer);
            }

            ProducerDTO producerDTO = config.Map<ProducerViewModel, ProducerDTO>(producer);

            producerService.Create(producerDTO);

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Producer created successful!";

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

            if (producerService.IsExistProducer(config.Map<ProducerViewModel, ProducerDTO>(producer)))
            {
                ModelState.AddModelError("", "Such producer already exist!");
                return View(producer);
            }

            producerService.Edit(config.Map<ProducerViewModel, ProducerDTO>(producer));

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Producer edited successful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsProducer(int id)
        {
            ProducerViewModel producer = config.Map<ProducerDTO, ProducerViewModel>(producerService.Get(id));

            if (producer == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
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
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This producer isn't exist";
                return RedirectToAction("Index");
            }

             ViewBag.CountProducts = producerService.GetCountProductsByProducerId(id);

            return View(producer);
        }

        [HttpPost]
        public ActionResult DeleteProducer(ProducerViewModel producer)
        {
            if (producer == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This producer isn't exist";
                return RedirectToAction("Index");
            }

            producerService.Delete(producer.Id);

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Producer deleted successful!";

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

            XmlElement producers = xmlFile.DocumentElement;

            if (producers.Name != "producers")
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "File XML don't exist producers root";
                return RedirectToAction("Index");
            }

            int countUpload = 0;
            int countNotUpload = 0;

            foreach (XmlNode producer in producers)
            {
                if (!producerService.IsExistProducer(producer.InnerText))
                {
                    producerService.Create(producer.InnerText);
                    countUpload++;
                }
                else
                {
                    countNotUpload++;
                }
            }

            saveXMLService.SaveXML(file, "Producers");

            TempData["StatusMessage"] = StateMessage.info.ToString();
            TempData["Message"] = $"{countUpload} producer's upload successful and {countNotUpload} is not";

            return RedirectToAction("Index");
        }
    }
}