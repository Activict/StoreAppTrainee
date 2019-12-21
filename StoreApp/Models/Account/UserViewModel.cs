using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.Models.Account
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string HomeAddress { get; set; }
        public string Role { get; set; }
    }
}