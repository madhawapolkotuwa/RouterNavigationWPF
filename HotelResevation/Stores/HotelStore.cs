using HotelResevation.Models;
using System.IO;
using System.Text.Json;

namespace HotelResevation.Stores
{
    public class HotelStore
    {
        private Lazy<Task> _initializeLazy;
        private List<Hotel> _hotelList;

        public Hotel SelectedHotel { get; set; }

        public IEnumerable<Hotel> HotelList => _hotelList;
        public HotelStore()
        {
            _hotelList = new List<Hotel>();
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        private async Task Initialize()
        {
            var jsonFilePath = "Resources/HotelData.json";
            // Load and deserialize the JSON data to a list of Hotel objects
            var jsonData = File.ReadAllText(jsonFilePath);
            var hotelData = JsonSerializer.Deserialize<HotelData>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            _hotelList = hotelData?.Value;
        }

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(Initialize);
                throw;
            }
        }
    }
}
