using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    class OrderService : IOrderService
    {
        IUnitOfWork DataBase;
        public OrderService(IUnitOfWork eof)
        {
            DataBase = eof;
        }
        public void Create(OrderDTO order)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()).CreateMapper();
            Order orderDAL = config.Map<OrderDTO, Order>(order);
            DataBase.Orders.Create(orderDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Orders.Delete(id);
            DataBase.Save();
        }

        public void Edit(OrderDTO order)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()).CreateMapper();
            Order orderDAL = config.Map<OrderDTO, Order>(order);
            DataBase.Orders.Update(orderDAL);
            DataBase.Save();
        }

        public OrderDTO Get(int id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()).CreateMapper();
            return config.Map<Order, OrderDTO>(DataBase.Orders.Get(id));
        }

        public IEnumerable<OrderDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()).CreateMapper();
            return config.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(DataBase.Orders.GetAll());
        }
        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
