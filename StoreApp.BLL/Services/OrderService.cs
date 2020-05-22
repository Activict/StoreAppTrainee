using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork database;
        private IMapper config;

        public OrderService(IUnitOfWork uof)
        {
            database = uof;
            config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<OrderDTO, Order>(); 
                    cfg.CreateMap<Order, OrderDTO>();
                }).CreateMapper();
        }

        public void Create(OrderDTO order)
        {
            var orderDAL = config.Map<OrderDTO, Order>(order);
            database.Orders.Create(orderDAL);
            database.Save();
        }

        public int Create(int userId, decimal totalPrice, int discount = 0, string status = "done")
        {
            var orderDTO = new OrderDTO()
            {
                UserId = userId,
                TotalCost = totalPrice,
                Discount = discount,
                Status = status,
                OrderDate = DateTime.Now
            };

            var orderDAL = config.Map<OrderDTO, Order>(orderDTO);
            database.Orders.Create(orderDAL);
            database.Save();
            return orderDAL.Id;
        }

        public void Delete(int id)
        {
            database.Orders.Delete(id);
            database.Save();
        }

        public void Edit(OrderDTO order)
        {
            var orderDAL = config.Map<OrderDTO, Order>(order);
            database.Orders.Update(orderDAL);
            database.Save();
        }

        public OrderDTO Get(int id)
        {
            return config.Map<Order, OrderDTO>(database.Orders.Get(id));
        }

        public IEnumerable<OrderDTO> GetAll()
        {
            return config.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(database.Orders.GetAll());
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
