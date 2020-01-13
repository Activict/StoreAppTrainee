using StoreApp.BLL.DTO;

namespace StoreApp.BLL.Interfaces
{
    public interface IUserValidateService
    {
        bool CheckEmail(string email);
        bool CheckUserName(string userName);
        bool CheckLogin(UserDTO userDTO);
        string GetRole(UserDTO userDTO);
        bool IsTruePassword(UserDTO userDTO);
        bool IsExistUser(UserDTO userDTO);
    }
}
