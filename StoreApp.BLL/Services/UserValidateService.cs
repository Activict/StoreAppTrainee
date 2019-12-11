using StoreApp.BLL.DTO;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;

namespace StoreApp.BLL.Services
{
    public class UserValidateService
    {
        private IUnitOfWork DataBase { get; set; }

        public UserValidateService()
        {
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public bool CheckEmail(string email)
        {
            var users = DataBase.Users.GetAll();

            foreach (var user in users)
            {
                if (user.Email == email)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckUserName(string userName)
        {
            var users = DataBase.Users.GetAll();

            foreach (var user in users)
            {
                if (user.UserName == userName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckLogin(UserDTO userDTO)
        {
            var users = DataBase.Users.GetAll();

            foreach (var user in users)
            {
                if (user.UserName == userDTO.UserName && user.Password == userDTO.Password)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetRole(UserDTO userDTO)
        {
            var users = DataBase.Users.GetAll();

            foreach (var user in users)
            {
                if (user.UserName == userDTO.UserName && user.Password == userDTO.Password)
                {
                    return user.Role;
                }
            }

            return null;
        }
    }
}
