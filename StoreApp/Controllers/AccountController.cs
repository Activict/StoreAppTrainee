using System.Web.Mvc;
using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Account;

namespace StoreApp.Controllers
{
    public class AccountController : Controller
    {
        private UserValidateService userValidateService;
        private UserService userService;
        private IMapper config;
        public AccountController()
        {
            userValidateService = new UserValidateService();
            userService = new UserService();
            config = new MapperConfiguration(cfg => cfg.CreateMap<UserLoginViewModel, UserDTO>()).CreateMapper();
        }
        
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.User = new UserRegistrationViewModel();
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Your registraition wrong!";
                return View(model);
            }

            if (!userValidateService.CheckEmail(model.Email) &&
                !userValidateService.CheckUserName(model.Username))
            {
                UserDTO userDTO = new UserDTO() { Email = model.Email,
                                                  Password = model.Password,
                                                  UserName = model.Username,
                                                  HomeAddress = model.HomeAddress,
                                                  Role = "user"};
                
                userService.Create(userDTO);

                TempData["Message"] = "Your registraition successful!";
            }
            else
            {
                ViewBag.Message = "This is Email or Username already exist!";
                return View();
            }
            
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserDTO userDTO = config.Map<UserLoginViewModel, UserDTO>(model);

            if (userValidateService.CheckLogin(userDTO))
            {
                Session["Role"] = userValidateService.GetRole(userDTO);
                TempData["Message"] = "You login successful!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login or password wrong!");
                return View();
            }
        }
    }
}