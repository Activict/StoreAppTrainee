using StoreApp.Models.Orders;
using System.Xml.Linq;

namespace StoreApp.Util
{
    public interface IOrderManager
    {
        void GetOrderDatails(OrderViewModel order);
        XDocument GetOrderById(int id);
    }
}
