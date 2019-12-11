using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Account
{
    public class UserLoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}