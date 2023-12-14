using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using flightMVC.Models;

namespace flightMVC.Models
{
    public class Aircraft
    {
        public int Id { get; set; }

        [Required]
        public int AircraftModelId { get; set; }
        [Required]
        public int CompanyId { get; set; }

        // Navigation property for Company
        [ForeignKey("AircraftModelId")]
        public AircraftModel AircraftModel { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
}
