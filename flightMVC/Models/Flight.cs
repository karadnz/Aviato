﻿using System;
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
        public Route Route { get; set; }
        [ForeignKey("AircraftId")]
        public Aircraft Aircraft { get; set; }

        // Additional fields can be added as needed
    }
}