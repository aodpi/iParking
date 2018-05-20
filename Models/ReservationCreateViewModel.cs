using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iParking.Models
{
    public class ReservationCreateViewModel
    {
        public int ParkingId { get; set; }
        [Required, Display(Name = "Numar de inmatriculare")]
        public string CarNumber { get; set; }

        [Required]
        public string CarCategory { get; set; }
        public DateTime ParkingDate { get; set; } = DateTime.Now;
        public int ParkingDuration { get; set; }
        public decimal AmountPaid { get; set; }
        public List<Parking> AvailableParkings { get; set; }

        public IEnumerable<string> CarCategories { get; set; } = new List<string>
        {
            "A",
            "A1",
            "B",
            "BE",
            "C1"
        };
    }
}
