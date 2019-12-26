using StoreApp.BLL.DTO;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class UserValidateService
    {
        private IUnitOfWork DataBase { get; set; }

        public UserValidateService()
        {
            DataBase = new EFUnitOfWork("DefaultConnection");
        }
        public UserValidateService(IUnitOfWork uof)
        {
            DataBase = uof;
        }

        public bool CheckEmail(string email)
        {
            var users = DataBase.Users.GetAll();

            return users.Any(u => u.Email.Equals(email));
        }

        public bool CheckUserName(string userName)
        {
            var users = DataBase.Users.GetAll();

            return users.Any(u => u.UserName.Equals(userName));
        }

        public bool CheckLogin(UserDTO userDTO)
        {
            var users = DataBase.Users.GetAll();

            return users.Any(u => u.UserName.Equals(userDTO.UserName) && u.Password.Equals(userDTO.Password));
        }

        public string GetRole(UserDTO userDTO)
        {
            var users = DataBase.Users.GetAll();

            return users.FirstOrDefault(u => u.UserName.Equals(userDTO.UserName) && u.Password.Equals(userDTO.Password))?.Role;
        }

        public bool IsTruePassword(UserDTO userDTO)
        {
            return DataBase.Users.GetAll()
                .Any(u => u.Id.Equals(userDTO.Id) &&
                u.Password.Equals(userDTO.Password));
        }

        public bool IsExistUser(UserDTO userDTO)
        {
            return !DataBase.Users.GetAll()
                .Any(u => u.Id != userDTO.Id &&
                (u.UserName.Equals(userDTO.UserName) ||
                u.Email.Equals(userDTO.Email)));
        }
    }
}
