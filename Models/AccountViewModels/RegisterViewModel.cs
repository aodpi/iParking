using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iParking.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmare parola")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "Nume")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Prenume")]
        public string LastName { get; set; }

        [Required, Display(Name = "Zi de nastere")]
        public DateTime BirthDate { get; set; }

        [Required, Display(Name = "Gen")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Nume de utilizator")]
        public string UserName { get; set; }
    }
}
