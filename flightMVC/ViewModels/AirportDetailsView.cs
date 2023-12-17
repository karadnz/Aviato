using flightMVC.Models;
using System.Collections.Generic;
using Route = flightMVC.Models.Route;

namespace flightMVC.ViewModels
{
    public class AirportDetailsViewModel
    {
        public Airport Airport { get; set; }
        public IDictionary<Route, int> DepartureRoutes { get; set; }
        public IDictionary<Route, int> ArrivalRoutes { get; set; }

        // Add additional fields if needed
    }
}
