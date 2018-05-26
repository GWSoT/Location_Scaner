using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{

    public class RegisterDto
    {
        [Required(ErrorMessage = "First name must be specified.")]
        [Display(Description = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name must be specified.")]
        [Display(Description = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth must be specified.")]
        [Display(Description = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email must be specified.")]
        [Display(Description = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be specified.")]
        [Display(Description = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password did not match")]
        [Display(Description = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
