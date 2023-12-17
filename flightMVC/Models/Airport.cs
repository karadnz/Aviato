using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flightMVC.Models
{
    public class Airport
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }



        //navigation?
        public virtual ICollection<Route>? DepartureRoutes { get; set; }
        public virtual ICollection<Route>? ArrivalRoutes { get; set; }
    }
}
