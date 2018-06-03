using System.ComponentModel.DataAnnotations;

namespace iParking.Models
{
    public class WalletFillModel
    {
        [Required]
        public int Amount { get; set; }

        [Required, Display(Name = "Nume pe card")]
        public string CardOwner { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required, Display(Name = "Data expirării")]
        public string CardExpiryMonth { get; set; }

        [Required]
        public string CardExpiryYear { get; set; }

        [Required, Display(Name = "Cod de securitate")]
        public string CVV { get; set; }
    }
}
