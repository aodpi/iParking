using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace iParking.Models
{
    [DebuggerDisplay("Total: {Amount, nq}")]
    public class UserWallet
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public decimal Amount { get; set; }
    }
}
