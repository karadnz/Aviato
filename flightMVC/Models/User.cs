using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flightMVC.Models
{
	public class User
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }


    }
}

