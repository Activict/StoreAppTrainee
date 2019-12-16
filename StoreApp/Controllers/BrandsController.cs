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
        private BrandService brandsService;
        private IMapper config;

        public BrandsController()
        {
            brandsService = new BrandService();
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandDTO, BrandViewModel>();
                cfg.CreateMap<BrandViewModel, BrandDTO>();
            }).CreateMapper();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var brands = config.Map<IEnumerable<BrandDTO>, IEnumerable<BrandViewModel>>(brandsService.GetAll());
            
            return View(brands);
        }
    }
}