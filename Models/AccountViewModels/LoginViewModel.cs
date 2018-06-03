using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iParking.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nume utilizator")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Parolă")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Memorează-mă?")]
        public bool RememberMe { get; set; }
    }
}
