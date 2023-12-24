using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flightMVC.Models
{
	public class Booking
	{
        [Key]
        public int Id { get; set; }


        [Required]
        public DateTime PurchaseTime { get; set; } //

        [Required]
        public int SeatNumber { get; set; }

        // Foreign keys
        [Required]
        public int FlightId { get; set; }
        [Required]
        public int UserId { get; set; }

        // Navigation property
        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }


        // Additional fields can be added as needed
        public void ConvertTimesToUtc()
        {
            TimeZoneInfo gmtPlus3Zone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            PurchaseTime = TimeZoneInfo.ConvertTimeToUtc(PurchaseTime, gmtPlus3Zone);
            
        }


    }
}

