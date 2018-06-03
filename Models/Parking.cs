using System.ComponentModel.DataAnnotations;

namespace iParking.Models
{
    public class Parking
    {
        public int Id { get; set; }

        [Display(Name = "Nume parcare")]
        public string ParkingName { get; set; }

        [Display(Name = "Număr parcare")]
        public int ParkingNumber { get; set; }

        [Display(Name = "Locuri parcare")]
        public int ParkingSlots { get; set; }

        [Display(Name = "Preț pe o oră")]
        public decimal PricePerHour { get; set; }

        [Display(Name = "Latitudine")]
        public double Latitude { get; set; }
        
        [Display(Name = "Longitudine")]
        public double Longitude { get; set; }
    }
}
