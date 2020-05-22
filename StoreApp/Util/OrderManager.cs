using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.Models.OrderDetails;
using StoreApp.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace StoreApp.Util
{
    public class OrderManager : IOrderManager
    {
        private IWebMapper webMapper;
        private IOrderService orderService;
        private IUserService userService;
        private IOrderDetailService orderDetailService;
        private IProductService productService;

        public OrderManager(
            IWebMapper mapper,
            IOrderService order,
            IUserService user,
            IOrderDetailService orderDetail,
            IProductService product)
        {
            webMapper = mapper;
            orderService = order;
            userService = user;
            orderDetailService = orderDetail;
            productService = product;
        }

        public void GetOrderDatails(OrderViewModel order)
        {
            var orderDetailsDTOs = orderDetailService.GetAll().Where(o => o.OrderId == order.Id);
            order.OrderDetails = webMapper.Config.Map<IEnumerable<OrderDetailDTO>, IEnumerable<OrderDetailsViewModel>>(orderDetailsDTOs);

            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetail.Product = webMapper.Map(productService.Get(orderDetail.ProductId));
            }
        }

        public XDocument GetOrderById(int id)
        {
            var order = webMapper.Config.Map<OrderDTO, OrderViewModel>(orderService.Get(id));
            GetOrderDatails(order);

            var user = userService.Get(order.UserId);

            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "WINDOWS-1251", null),
                    new XElement("FileOrder",
                    new XAttribute("Id", Guid.NewGuid().ToString()),
                    new XAttribute("Date", DateTime.Now.ToString()),
                    new XAttribute("AppVersion", typeof(Controller).Assembly.GetName().Version.ToString()),
                new XElement("Order",
                    new XAttribute("Id", order.Id.ToString()),
                    new XAttribute("Date", order.OrderDate.ToString()),
                    new XAttribute("Discount", order.Discount.ToString()),
                    new XAttribute("Cost", order.TotalCost.ToString()),
                    new XElement("User",
                        new XAttribute("Name", user.UserName),
                        new XAttribute("Email", user.Email),
                        new XAttribute("HomeAddress", user.HomeAddress)),
                    new XElement("Products",
                    order.OrderDetails.Select(ord => 
                        new XElement("Product", 
                            new XAttribute("Name", ord.Product.Name),
                            new XAttribute("Producer", ord.Product.Producer),
                            new XAttribute("Brand", ord.Product.Brand),
                            new XAttribute("Category", ord.Product.Category),
                            new XAttribute("Unit", ord.Product.Unit),
                            new XAttribute("Price", ord.Product.Price),
                            new XAttribute("Quantity", ord.Product.Quantity)
                ))))));

            return xmlDocument;
        }
    }
}