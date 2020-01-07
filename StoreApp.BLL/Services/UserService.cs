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
        private IUnitOfWork database;
        private IMapper config;

        public UserService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserDTO, User>();
                    cfg.CreateMap<User, UserDTO>();
                }).CreateMapper();
            database = uof;
        }

        public UserService()
        {
            config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserDTO, User>();
                    cfg.CreateMap<User, UserDTO>();
                }).CreateMapper();
            database = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(UserDTO user)
        {
            User userDAL = config.Map<UserDTO, User>(user);
            database.Users.Create(userDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.Users.Delete(id);
            database.Save();
        }

        public void Edit(UserDTO user)
        {
            var userDTO = config.Map<UserDTO, User>(user);
            database.Users.Update(userDTO);
            database.Save();
        }

        public UserDTO Get(int id)
        {
            return config.Map<User, UserDTO>(database.Users.Get(id));
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return config.Map<IEnumerable<User>, IEnumerable<UserDTO>>(database.Users.GetAll());
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
