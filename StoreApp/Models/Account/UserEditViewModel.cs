using System.ComponentModel.DataAnnotations;

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
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Username { get; set; }
        public string HomeAddress { get; set; }
        public string Role { get; set; }
    }
}