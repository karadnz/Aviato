using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using flightMVC.Models;
using Route = flightMVC.Models.Route;

namespace flightMVC.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        // Foreign key for Route
        [Required]
        public int RouteId { get; set; }
        [Required]
        public int AircraftId { get; set; }

        // Navigation property
        [ForeignKey("RouteId")]
        public Route? Route { get; set; }
        [ForeignKey("AircraftId")]
        public Aircraft? Aircraft { get; set; }


        //nav
        public virtual ICollection<Booking>? Bookings { get; set; }

        // Additional fields can be added as needed
        public void ConvertTimesToUtc()
        {
            TimeZoneInfo gmtPlus3Zone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DepartureTime = TimeZoneInfo.ConvertTimeToUtc(DepartureTime, gmtPlus3Zone);
            ArrivalTime = TimeZoneInfo.ConvertTimeToUtc(ArrivalTime, gmtPlus3Zone);
        }
    }
}