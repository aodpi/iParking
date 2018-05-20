using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iParking.Models
{
    public class UserAccountEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public string Nume { get; set; }

        [Required]
        public string Prenume { get; set; }

        [Required]
        public string IDNP { get; set; }

        [DataType(DataType.Date), Required]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }
    }
}
