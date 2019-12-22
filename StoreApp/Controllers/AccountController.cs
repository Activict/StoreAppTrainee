using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Account;
using StoreApp.Util;

namespace StoreApp.Controllers
{
    public class AccountController : Controller
    {
        private UserValidateService userValidateService;
        private UserService userService;
        private WebMapper webMapper;

        public AccountController()
        {
            userValidateService = new UserValidateService();
            userService = new UserService();
            webMapper = new WebMapper();
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            if (Session["Role"] as string == "admin" ) 
            {
                var users = webMapper.config.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(userService.GetAll());
                return View(users);
            }

            return RedirectToAction("Index", "Store", null);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Your registration wrong!";
                return View();
            }

            if (!userValidateService.CheckEmail(model.Email) &&
                !userValidateService.CheckUserName(model.Username))
            {
                UserDTO userDTO = webMapper.config.Map<UserRegistrationViewModel, UserDTO>(model);
                userDTO.Role = "user";

                userService.Create(userDTO);

                TempData["Message"] = "Your registration successful!";
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
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Account");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserDTO userDTO = webMapper.config.Map<UserLoginViewModel, UserDTO>(model);

            if (userValidateService.CheckLogin(userDTO))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                Session["Role"] = userValidateService.GetRole(userDTO);
                TempData["Message"] = "You login successful!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login or password wrong!");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Account()
        {
            if (Request.IsAuthenticated)
            {
                UserDTO userDTO = userService.GetAll().FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                UserViewModel user = webMapper.config.Map<UserDTO, UserViewModel>(userDTO);

                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditUser(int id)
        {
            UserEditViewModel user = webMapper.config.Map<UserDTO, UserEditViewModel>(userService.Get(id));

            if (user != null && user.Username.Equals(User.Identity.Name))
            {
                return View(user);
            }

            return RedirectToAction("Account");
        }

        [HttpPost]
        public ActionResult EditUser(UserEditViewModel userEdit)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Your edit account wrong!");
                return View(userEdit);
            }

            UserDTO userDTO = webMapper.config.Map<UserEditViewModel, UserDTO>(userEdit);

            if (!userValidateService.CheckTruePassword(userDTO))
            {
                ModelState.AddModelError("", "Your enter wrong password!");
                return View(userEdit);
            }

            if (userValidateService.CheckForEditUser(userDTO))
            {
                userService.Edit(userDTO);

                TempData["Message"] = "Your edit account successful!";

                return RedirectToAction("Account");
            }
            else
            {
                ModelState.AddModelError("", "This is Email or Username already exist!");
                return View(userEdit);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DetailsUser(int id)
        {
            if (Session["Role"] as string == "admin")
            {
                var user = webMapper.config.Map<UserDTO, UserViewModel>(userService.Get(id));
                return View(user);
            }

            return RedirectToAction("Account", "Account", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            TempData["Message"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}