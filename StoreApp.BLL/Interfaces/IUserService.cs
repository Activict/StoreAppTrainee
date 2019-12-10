using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IUserService
    {
        void Create(UserDTO user);
        void Delete(int id);
        void Edit(UserDTO user);
        UserDTO Get(int id);
        IEnumerable<UserDTO> GetAll();
        void Dispose();
    }
}
