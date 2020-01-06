using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using StoreApp.Models.Account;
using StoreApp.Util;
using StoreApp.Models.Orders;
using StoreApp.Models.OrderDetails;
using StoreApp.Models.Store;
using StoreApp.Enums;

namespace StoreApp.Controllers
{
    public class AccountController : Controller
    {
        private UnitService unitService;
        private ProductService productService;
        private OrderService orderService;
        private OrderDetailService orderDetailService;
        private UserValidateService userValidateService;
        private UserService userService;
        private WebMapper webMapper;

        public AccountController()
        {
            unitService = new UnitService();
            productService = new ProductService();
            orderService = new OrderService();
            orderDetailService = new OrderDetailService();
            userValidateService = new UserValidateService();
            userService = new UserService();
            webMapper = new WebMapper();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            if ((Session["Role"] as string).Equals(UserRoles.Admin.ToString()))
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
                userDTO.Role = UserRoles.User.ToString();

                userService.Create(userDTO);
                TempData["StatusMessage"] = StateMessage.success.ToString();
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
                TempData["StatusMessage"] = StateMessage.success.ToString();
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
        public ActionResult UserOrders(int? id)
        {
            var orders = webMapper.config.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(orderService.GetAll().Where(o => id == null ? o.UserId != id : o.UserId == id));

            orders.ToList().ForEach(o => GetOrderDatails(o));

            return View(orders);
        }

        private void GetOrderDatails(OrderViewModel order)
        {
            var orderDetailsDTOs = orderDetailService.GetAll().Where(o => o.OrderId == order.Id);
            order.OrderDetails = webMapper.config.Map<IEnumerable<OrderDetailDTO>, IEnumerable<OrderDetailsViewModel>>(orderDetailsDTOs);
            
            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetail.Product = webMapper.config.Map<ProductDTO, ProductViewModel>(productService.Get(orderDetail.ProductId));
                orderDetail.Product.Unit = unitService.Get(orderDetail.Product.UnitId).Name;
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

            if (!userValidateService.IsTruePassword(userDTO))
            {
                ModelState.AddModelError("", "Your enter wrong password!");
                return View(userEdit);
            }

            if (userValidateService.IsExistUser(userDTO))
            {
                userService.Edit(userDTO);
                TempData["StatusMessage"] = StateMessage.success.ToString();
                TempData["Message"] = "The user was updated successfully";

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
            if ((Session["Role"] as string).Equals(UserRoles.Admin.ToString()))
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
            if ((Session["Role"] as string).Equals(UserRoles.Admin.ToString()))
            {
                UserViewModel user = webMapper.config.Map<UserDTO, UserViewModel>(userService.Get(id));

                if (user != null)
                {
                    List<SelectListItem> ListOfRoles = new List<SelectListItem>();
                    ListOfRoles.Add(new SelectListItem() { Text = UserRoles.User.ToString(), Value = ((int)UserRoles.User).ToString() });
                    ListOfRoles.Add(new SelectListItem() { Text = UserRoles.Admin.ToString(), Value = ((int)UserRoles.Admin).ToString() });

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
            if ((Session["Role"] as string).Equals(UserRoles.Admin.ToString()))
            {
                user.Role = int.Parse(user.Role) == (int)(UserRoles.User) ? UserRoles.User.ToString() 
                                                                          : UserRoles.Admin.ToString();

                UserDTO userDTO = webMapper.config.Map<UserViewModel, UserDTO>(user);

                userService.Edit(userDTO);

                TempData["StatusMessage"] = StateMessage.success.ToString();
                TempData["Message"] = $"{user.UserName} edit successfully";

                return RedirectToAction("DetailsUser", "Account", new { id = user.Id });
            }

            TempData["StatusMessage"] = StateMessage.danger.ToString();
            TempData["Message"] = $"{user.UserName} wasn't edit";

            return RedirectToAction("Account", "Account");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            if ((Session["Role"] as string).Equals(UserRoles.Admin.ToString()))
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
            if ((Session["Role"] as string).Equals(UserRoles.Admin.ToString()))
            {
                if (user != null)
                {
                    userService.Delete(user.Id);
                    TempData["StatusMessage"] = StateMessage.success.ToString();
                    TempData["Message"] = $"{user.UserName} deleted successfully";
                }
                else
                {
                    TempData["StatusMessage"] = StateMessage.danger.ToString();
                    TempData["Message"] = $"{user.UserName} wasn't delete";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            TempData["StatusMessage"] = null;
            TempData["Message"] = null;
            Session["Role"] = null;
            TempData["Cart"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}