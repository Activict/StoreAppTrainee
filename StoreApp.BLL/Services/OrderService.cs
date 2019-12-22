using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public OrderService(IUnitOfWork eof)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()).CreateMapper();
            DataBase = eof;
        }

        public OrderService()
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(OrderDTO order)
        {
            Order orderDAL = config.Map<OrderDTO, Order>(order);
            DataBase.Orders.Create(orderDAL);
            DataBase.Save();
        }

        public int Create(int userId, decimal totalPrice, int discount = 0, string status = "done")
        {
            OrderDTO orderDTO = new OrderDTO()
            {
                UserId = userId,
                TotalCost = totalPrice,
                Discount = discount,
                Status = status,
                OrderTime = DateTime.Now
            };

            Order orderDAL = config.Map<OrderDTO, Order>(orderDTO);
            DataBase.Orders.Create(orderDAL);
            DataBase.Save();
            return orderDAL.Id;
        }

        public void Delete(int id)
        {
            DataBase.Orders.Delete(id);
            DataBase.Save();
        }

        public void Edit(OrderDTO order)
        {
            Order orderDAL = config.Map<OrderDTO, Order>(order);
            DataBase.Orders.Update(orderDAL);
            DataBase.Save();
        }

        public OrderDTO Get(int id)
        {
            return config.Map<Order, OrderDTO>(DataBase.Orders.Get(id));
        }

        public IEnumerable<OrderDTO> GetAll()
        {
            return config.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(DataBase.Orders.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
