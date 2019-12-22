using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.Account
{
    public class UserRegistrationViewModel
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
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string UserName { get; set; }
        public string HomeAddress { get; set; }
    }
}