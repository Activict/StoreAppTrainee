using System.Collections.Generic;
using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;

namespace StoreApp.BLL.Services
{
    class UserService : IUserService
    {
        IUnitOfWork Database;
        public UserService(IUnitOfWork uof)
        {
            Database = uof;
        }
        public void Create(UserDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            User userDAL = config.Map<UserDTO, User>(user);
            Database.Users.Create(userDAL);
            Database.Save();
        }

        public void Delete(int id)
        {
            Database.Users.Delete(id);
            Database.Save();
        }

        public void Edit(UserDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            User userDTO = config.Map<UserDTO, User>(user);
            Database.Users.Update(userDTO);
            Database.Save();
        }

        public UserDTO Get(int id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return config.Map<User, UserDTO>(Database.Users.Get(id));
        }

        public IEnumerable<UserDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return config.Map<IEnumerable<User>, IEnumerable<UserDTO>>(Database.Users.GetAll());
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
