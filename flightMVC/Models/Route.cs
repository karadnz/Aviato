using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using flightMVC.Models;

namespace flightMVC.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Required]
        public int DepartureAirportId { get; set; }
        [Required]
        public int ArrivalAirportId { get; set; }
        [Required]
        public string RouteCode { get; set; }


        [ForeignKey("DepartureAirportId")]
        [InverseProperty("DepartureRoutes")]
        public Airport? DepartureAirport { get; set; }
        [ForeignKey("ArrivalAirportId")]
        [InverseProperty("ArrivalRoutes")]
        public Airport? ArrivalAirport { get; set; }

        //Navigation
        public virtual ICollection<Flight>? Flights { get; set; }
    }
}