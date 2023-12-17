using flightMVC.Models;

namespace flightMVC.ViewModels
{
    public class AircraftDetailsViewModel
    {
        public Aircraft Aircraft { get; set; }
        public AircraftModel AircraftModel { get; set; }
        public Company Company { get; set; }
    }
}
