using flightMVC.Models;

namespace flightMVC.ViewModels
{
    public class BookingCreateViewModel
    {
        public Booking Booking { get; set; }
        public Flight Flight { get; set; }
        public Aircraft Aircraft { get; set; }
        public AircraftModel AircraftModel { get; set; }


        // Additional properties can be added as needed
    }
}