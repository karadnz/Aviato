using System;
using System.ComponentModel.DataAnnotations;
using flightMVC.Models;

namespace flightMVC.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }

        //Navigation
        public ICollection<Aircraft>? Aircrafts { get; set; } // Represents a one-to-many relationship with Aircraft
    }
}

