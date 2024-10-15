namespace HotelResevation.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string Image { get; set; }
        public string HotelName {  get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool ParkingIncluded { get; set; }
        public double Rating { get; set; }
        public Address Address { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
