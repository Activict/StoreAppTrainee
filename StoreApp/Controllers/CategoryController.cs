using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Categories;
using System.Collections.Generic;
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

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryDTO, CategoryViewModel>();
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

            TempData["StatusMessage"] = "success";
            TempData["Message"] = "Category created successful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            CategoryViewModel category = config.Map<CategoryDTO, CategoryViewModel>(categoryService.Get(id));

            if (category == null)
            {
                RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryViewModel category)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Category edited wrong!");
                return View(category);
            }

            categoryService.Edit(config.Map<CategoryViewModel, CategoryDTO>(category));

            TempData["StatusMessage"] = "success";
            TempData["Message"] = "Category edited successful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsCategory(int id)
        {
            CategoryViewModel category = config.Map<CategoryDTO, CategoryViewModel>(categoryService.Get(id));

            if (category == null)
            {
                TempData["StatusMessage"] = "danger";
                TempData["Message"] = "This category isn't exist";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            CategoryViewModel category = config.Map<CategoryDTO, CategoryViewModel>(categoryService.Get(id));

            if (category == null)
            {
                TempData["StatusMessage"] = "danger";
                TempData["Message"] = "This category isn't exist";
                return RedirectToAction("Index");
            }

            ViewBag.CountProducts = categoryService.GetCountProductsByCategoryId(id);

            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(CategoryViewModel category)
        {
            if (category == null)
            {
                TempData["StatusMessage"] = "danger";
                TempData["Message"] = "This category isn't exist";
                return RedirectToAction("Index");
            }

            categoryService.Delete(category.Id);

            TempData["StatusMessage"] = "success";
            TempData["Message"] = "Category deleted successful!";

            return RedirectToAction("Index");
        }
    }
}