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
    }
}