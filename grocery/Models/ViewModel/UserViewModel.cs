using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace grocery.Models.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username Required")]

        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string FullName { get; set; }
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Mismatch")]
        public string ConfirmPassword { get; set; }
        public string Photo { get; set; }
        public string Usertype { get; set; }
        public string Username { get; set; }
    }
}