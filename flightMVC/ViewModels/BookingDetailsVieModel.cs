using flightMVC.Models;

namespace flightMVC.ViewModels
{
    public class BookingDetailsViewModel
    {
        public Booking Booking { get; set; }
        public Flight Flight { get; set; }
        public User User { get; set; }

        // Additional properties can be added as needed

        public AircraftModel? AircraftModel { get; set; }
        public List<int>? BookedSeats { get; set; }
    }
}
