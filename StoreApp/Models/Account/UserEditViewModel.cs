using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StoreApp.Models.Account
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Old password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string UserName { get; set; }
        public string HomeAddress { get; set; }
        [Required]
        public string Role { get; set; }
    }
}