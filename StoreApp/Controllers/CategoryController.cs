using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Enums;
using StoreApp.Models.Categories;
using StoreApp.Util;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StoreApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private SaveXMLService saveXMLService;
        private CategoryService categoryService;

        private IMapper config;
        public CategoryController()
        {
            saveXMLService = new SaveXMLService();
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

            if (categoryService.IsExistCategory(config.Map<CategoryViewModel, CategoryDTO>(category)))
            {
                ModelState.AddModelError("", "Such category already exist!");
                return View(category);
            }

            CategoryDTO categoryDTO = config.Map<CategoryViewModel, CategoryDTO>(category);

            categoryService.Create(categoryDTO);

            TempData["StatusMessage"] = StateMessage.success.ToString();
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

            if (categoryService.IsExistCategory(config.Map<CategoryViewModel, CategoryDTO>(category)))
            {
                ModelState.AddModelError("", "Such category already exist!");
                return View(category);
            }

            categoryService.Edit(config.Map<CategoryViewModel, CategoryDTO>(category));

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Category edited successful!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsCategory(int id)
        {
            CategoryViewModel category = config.Map<CategoryDTO, CategoryViewModel>(categoryService.Get(id));

            if (category == null)
            {
                TempData["StatusMessage"] = StateMessage.danger.ToString();
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
                TempData["StatusMessage"] = StateMessage.danger.ToString();
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
                TempData["StatusMessage"] = StateMessage.danger.ToString();
                TempData["Message"] = "This category isn't exist";
                return RedirectToAction("Index");
            }

            categoryService.Delete(category.Id);

            TempData["StatusMessage"] = StateMessage.success.ToString();
            TempData["Message"] = "Category deleted successful!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadXML(HttpPostedFileBase file)
        {
            var fileManager = new FileManager(file, RootNames.categories);

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