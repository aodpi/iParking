using System;
using System.ComponentModel.DataAnnotations;

namespace iParking.Models
{
    public class ParkingReservation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required, Display(Name = "Număr mașină")]
        public string CarNumber { get; set; }

        [Required, Display(Name = "Categorie mașină")]
        public string CarCategory { get; set; }

        [Display(Name = "Dată parcare")]
        public DateTime ParkingDate { get; set; }

        [Display(Name = "Timp parcare")]
        public int ParkingTime { get; set; }

        [Display(Name = "Suma achitată")]
        public decimal AmountPaid { get; set; }
        public int ParkingId { get; set; }
        public Parking Parking { get; set; }
        public Guid VerificationCode { get; set; }

        public int SlotNumber { get; set; }
    }
}
