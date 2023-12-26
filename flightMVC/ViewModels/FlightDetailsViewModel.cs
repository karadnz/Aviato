using flightMVC.Models;
using Route = flightMVC.Models.Route;

namespace flightMVC.ViewModels
{
    public class FlightDetailsViewModel
    {
        public Flight Flight { get; set; }
        public Route Route { get; set; }
        public Aircraft Aircraft { get; set; }



        public Airport DepartureAirport { get; set; }
        public Airport ArrivalAirport { get; set; }


        public TimeSpan Duration { get; set; }

        // You might also want to format the duration as a string
        // for display purposes, e.g., "5 hours, 30 minutes"
        public string FormattedDuration
        {
            get
            {
                return $"{Duration.Hours} hours, {Duration.Minutes} minutes";
            }
        }

        public AircraftModel? AircraftModel { get; set; }
        public List<int>? BookedSeats { get; set; }

    }
}
