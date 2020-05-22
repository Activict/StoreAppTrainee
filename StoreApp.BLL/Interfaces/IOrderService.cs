using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IOrderService
    {
        void Create(OrderDTO order);
        int Create(int userId, decimal totalPrice, int discount = 0, string status = "done");
        void Delete(int id);
        void Edit(OrderDTO order);
        OrderDTO Get(int id);
        IEnumerable<OrderDTO> GetAll();
        void Dispose();
    }
}
