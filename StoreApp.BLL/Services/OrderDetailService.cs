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
        private IUnitOfWork database;
        private IMapper config;

        public OrderDetailService(IUnitOfWork eof)
        {
            config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<OrderDetailDTO, OrderDetail>();
                    cfg.CreateMap<OrderDetail, OrderDetailDTO>();
                }).CreateMapper();
            database = eof;
        }

        public OrderDetailService()
        {
            config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<OrderDetailDTO, OrderDetail>();
                    cfg.CreateMap<OrderDetail, OrderDetailDTO>();
                }).CreateMapper();
            database = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(OrderDetailDTO orderDetail)
        {
            OrderDetail orderDetailDAL = config.Map<OrderDetailDTO, OrderDetail>(orderDetail);
            database.OrderDetails.Create(orderDetailDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.OrderDetails.Delete(id);
            database.Save();
        }

        public void Edit(OrderDetailDTO orderDetail)
        {
            var orderDetailDAL = config.Map<OrderDetailDTO, OrderDetail>(orderDetail);
            database.OrderDetails.Update(orderDetailDAL);
            database.Save();
        }

        public OrderDetailDTO Get(int id)
        {
            return config.Map<OrderDetail, OrderDetailDTO>(database.OrderDetails.Get(id));
        }

        public IEnumerable<OrderDetailDTO> GetAll()
        {
            return config.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(database.OrderDetails.GetAll());
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
