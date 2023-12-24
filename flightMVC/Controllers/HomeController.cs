using flightMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Security.Claims;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        // GET: Home
        public IActionResult Index()
        {
            // Fetch distinct cities for departure and arrival options
            var cities = _context.Airports.Select(a => a.City).Distinct().ToList();

            // Pass the list of cities to the view
            ViewBag.DepartureCities = new SelectList(cities);
            ViewBag.ArrivalCities = new SelectList(cities);

            return View();
        }


        // POST: Search Flights
        [HttpPost]
        public IActionResult Flights(DateTime departureDate, string departureLocation, string arrivalLocation)
        {
            // Convert departureDate to UTC (or the appropriate time zone)
            // Assumption: departureDate is in local time
            TimeZoneInfo gmtPlus3Zone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTime utcDepartureDate = TimeZoneInfo.ConvertTimeToUtc(departureDate, gmtPlus3Zone);

            // Use the UTC date for comparison
            var targetDate = utcDepartureDate.Date;

            var flights = _context.Flights
                .Where(f =>  f.Route.DepartureAirport.City == departureLocation
                            && f.Route.ArrivalAirport.City == arrivalLocation)
                .ToList();
            //f.DepartureTime.Date == targetDate
                            //&&
            return View(flights);
        }


        // GET: Buy/5
        public IActionResult Buy(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var booking = new Booking { FlightId = id, PurchaseTime = DateTime.UtcNow, UserId = int.Parse(userIdClaim.Value) };
            return View(booking);
        }

        // POST: Buy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy([Bind("PurchaseTime,SeatNumber,FlightId,UserId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.ConvertTimesToUtc();
                _context.Add(booking);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }


    }
}
