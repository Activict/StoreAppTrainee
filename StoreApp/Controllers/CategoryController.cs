using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryService categoryService;

        private IMapper config;
        public CategoryController()
        {
            categoryService = new CategoryService();

            config = new MapperConfiguration(cfg => { cfg.CreateMap<CategoryDTO, CategoryViewModel>();
                                                      cfg.CreateMap<CategoryViewModel, CategoryDTO>();
                                                    }).CreateMapper();
        }
        public ActionResult Index()
        {
            var categories = config.Map<IEnumerable<CategoryDTO>, IEnumerable<CategoryViewModel>>(categoryService.GetAll());

            return View(categories);
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryViewModel category)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Category edit is wrong");
                return View(category);
            }

            CategoryDTO categoryDTO = config.Map<CategoryViewModel, CategoryDTO>(category);

            categoryService.Create(categoryDTO);

            TempData["Message"] = "Category created success!";

            return RedirectToAction("Index");
        }
    }
}