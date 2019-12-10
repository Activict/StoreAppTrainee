using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public OrderDetailService(IUnitOfWork eof)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailDTO, OrderDetail>()).CreateMapper();
            DataBase = eof;
        }

        public OrderDetailService()
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailDTO, OrderDetail>()).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(OrderDetailDTO orderDetail)
        {
            OrderDetail orderDetailDAL = config.Map<OrderDetailDTO, OrderDetail>(orderDetail);
            DataBase.OrderDetails.Create(orderDetailDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.OrderDetails.Delete(id);
            DataBase.Save();
        }

        public void Edit(OrderDetailDTO orderDetail)
        {
            OrderDetail orderDetailDAL = config.Map<OrderDetailDTO, OrderDetail>(orderDetail);
            DataBase.OrderDetails.Update(orderDetailDAL);
            DataBase.Save();
        }

        public OrderDetailDTO Get(int id)
        {
            return config.Map<OrderDetail, OrderDetailDTO>(DataBase.OrderDetails.Get(id));
        }

        public IEnumerable<OrderDetailDTO> GetAll()
        {
            return config.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(DataBase.OrderDetails.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
