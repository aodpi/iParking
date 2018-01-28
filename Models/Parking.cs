namespace iParking.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public string ParkingName { get; set; }
        public int ParkingNumber { get; set; }
        public int ParkingSlots { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
