using System.Collections.Generic;
using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;

namespace StoreApp.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public UserService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            DataBase = uof;
        }

        public UserService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(UserDTO user)
        {
            User userDAL = config.Map<UserDTO, User>(user);
            DataBase.Users.Create(userDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Users.Delete(id);
            DataBase.Save();
        }

        public void Edit(UserDTO user)
        {
            User userDTO = config.Map<UserDTO, User>(user);
            DataBase.Users.Update(userDTO);
            DataBase.Save();
        }

        public UserDTO Get(int id)
        {
            return config.Map<User, UserDTO>(DataBase.Users.Get(id));
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return config.Map<IEnumerable<User>, IEnumerable<UserDTO>>(DataBase.Users.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
