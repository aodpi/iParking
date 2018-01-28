using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iParking.Models
{
    public class WalletFillModel
    {
        [Required]
        public int Amount { get; set; }

        [Required, Display(Name = "Name on card")]
        public string CardOwner { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required, Display(Name = "Expiration Date")]
        public string CardExpiryMonth { get; set; }

        [Required]
        public string CardExpiryYear { get; set; }

        [Required, Display(Name = "CVV Code")]
        public string CVV { get; set; }
    }
}
