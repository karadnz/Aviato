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


        [ForeignKey("DepartureAirportId")]
        public Airport DepartureAirport { get; set; }
        [ForeignKey("ArrivalAirportId")]
        public Airport ArrivalAirport { get; set; }
    }
}
