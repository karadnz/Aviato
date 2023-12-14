using System.Collections.Generic;
using flightMVC.Models;

namespace flightMVC.ViewModels
{
    public class AircraftModelDetailsViewModel
    {
        public AircraftModel AircraftModel { get; set; }
        public Dictionary<Company, int> CompaniesWithModel { get; set; } = new Dictionary<Company, int>();
    }
}
