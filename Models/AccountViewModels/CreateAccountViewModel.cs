using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iParking.Models.AccountViewModels
{
    public class CreateAccountViewModel
    {
        [Required, Display(Name = "Nume utilizator")]
        public string UserName { get; set; }

        [Required, Display(Name = "Parola"), DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Parolele nu coincid"), Display(Name = "Confirmare Parola"), DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4)]
        public string ConfirmPassword { get; set; }
        public IEnumerable<ApplicationUser> ExistingUserNames { get; set; }

        public Dictionary<string, string> WalletAmounts { get; set; }

        [Display(Name = "Admin?")]
        public bool IsAdmin { get; set; }
    }
}
