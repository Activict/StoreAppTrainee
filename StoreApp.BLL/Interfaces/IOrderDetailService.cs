using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IOrderDetailService
    {
        void Create(OrderDetailDTO orderDetail);
        void Delete(int id);
        void Edit(OrderDetailDTO orderDetail);
        OrderDetailDTO Get(int id);
        IEnumerable<OrderDetailDTO> GetAll();
        void Dispose();
    }
}
