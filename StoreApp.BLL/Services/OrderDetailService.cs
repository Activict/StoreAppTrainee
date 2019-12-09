using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    class OrderDetailService : IOrderDetailService
    {
        IUnitOfWork Database;
        public OrderDetailService(IUnitOfWork eof)
        {
            Database = eof;
        }
        public void Create(OrderDetailDTO orderDetail)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailDTO, OrderDetail>()).CreateMapper();
            OrderDetail orderDetailDAL = config.Map<OrderDetailDTO, OrderDetail>(orderDetail);
            Database.OrderDetails.Create(orderDetailDAL);
            Database.Save();
        }

        public void Delete(int id)
        {
            Database.OrderDetails.Delete(id);
            Database.Save();
        }

        public void Edit(OrderDetailDTO orderDetail)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailDTO, OrderDetail>()).CreateMapper();
            OrderDetail orderDetailDAL = config.Map<OrderDetailDTO, OrderDetail>(orderDetail);
            Database.OrderDetails.Update(orderDetailDAL);
            Database.Save();
        }

        public OrderDetailDTO Get(int id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailDTO, OrderDetail>()).CreateMapper();
            return config.Map<OrderDetail, OrderDetailDTO>(Database.OrderDetails.Get(id));
        }

        public IEnumerable<OrderDetailDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetail, OrderDetailDTO>()).CreateMapper();
            return config.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(Database.OrderDetails.GetAll());
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
