using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SangitMIS.Models
{
    [MetadataType(typeof(UserRegisterMetaData))]
    public partial class UserRegister
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserRegisterMetaData
    {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password donot match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }




    }
}