
namespace HotelResevation.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
        public string Type  { get; set; }
        public double BaseRate { get; set; }
        public string BedOptions { get; set; }
        public int SleepsCount { get; set; }
        public bool SmokingAllowed { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
