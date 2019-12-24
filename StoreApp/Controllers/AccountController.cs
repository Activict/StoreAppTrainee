using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Account;
using StoreApp.Util;
using StoreApp.Enums;

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
            if ((Session["Role"] as string).Equals(UserRoles.admin.ToString()))
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
                !userValidateService.CheckUserName(model.UserName))
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

            if (user != null && user.UserName.Equals(User.Identity.Name))
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
            if ((Session["Role"] as string).Equals(UserRoles.admin.ToString()))
            {
                var user = webMapper.config.Map<UserDTO, UserViewModel>(userService.Get(id));
                return View(user);
            }

            return RedirectToAction("Account", "Account", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditUserAdmin(int id)
        {
            if ((Session["Role"] as string).Equals(UserRoles.admin.ToString()))
            {
                UserViewModel user = webMapper.config.Map<UserDTO, UserViewModel>(userService.Get(id));

                if (user != null)
                {
                    List<SelectListItem> ListOfRoles = new List<SelectListItem>();
                    ListOfRoles.Add(new SelectListItem() { Text = UserRoles.user.ToString(), Value = ((int)UserRoles.user).ToString() });
                    ListOfRoles.Add(new SelectListItem() { Text = UserRoles.admin.ToString(), Value = ((int)UserRoles.admin).ToString() });

                    ViewBag.ListOfRoles = new SelectList(ListOfRoles, "Value", "Text");

                    return View(user);
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Account", "Account", null);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditUserAdmin(UserViewModel user)
        {
            if ((Session["Role"] as string).Equals(UserRoles.admin.ToString()))
            {
                user.Role = user.Role.Equals((int)UserRoles.user) ? UserRoles.user.ToString() 
                                                                  : UserRoles.admin.ToString();

                UserDTO userDTO = webMapper.config.Map<UserViewModel, UserDTO>(user);

                userService.Edit(userDTO);

                TempData["Message"] = $"{user.UserName} edit success";

                return RedirectToAction("DetailsUser", "Account", new { id = user.Id });
            }

            TempData["Message"] = $"{user.UserName} don't edited";

            return RedirectToAction("Account", "Account");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            if ((Session["Role"] as string).Equals(UserRoles.admin.ToString()))
            {
                UserViewModel user = webMapper.config.Map<UserDTO, UserViewModel>(userService.Get(id));

                if (user != null)
                {
                    return View(user);
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Account", "Account", null);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteUser(UserViewModel user)
        {
            if ((Session["Role"] as string).Equals(UserRoles.admin.ToString()))
            {
                if (user != null)
                {
                    userService.Delete(user.Id);

                    TempData["Message"] = $"{user.UserName} deleted success";
                }
                else
                {
                    TempData["Message"] = $"{user.UserName} don't deleted";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            TempData["Message"] = null;
            TempData["Role"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}