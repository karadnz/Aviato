using flightMVC.Models;
using flightMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    [Authorize(Roles = "Admin, User")]
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

        [HttpPost]
        public IActionResult Flights(DateTime? departureDate, string departureLocation, string arrivalLocation)
        {
            var flightsQuery = _context.Flights.AsQueryable();

            // Check if departure date is provided
            if (departureDate.HasValue)
            {
                TimeZoneInfo gmtPlus3Zone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                DateTime utcDepartureDate = TimeZoneInfo.ConvertTimeToUtc((DateTime)departureDate, gmtPlus3Zone);
                var targetDate = utcDepartureDate.Date;
                var startOfNextDay = targetDate.AddDays(2);

                flightsQuery = flightsQuery.Where(f => f.DepartureTime >= targetDate && f.DepartureTime < startOfNextDay);
            }

            // Filter by departure location if provided
            if (!string.IsNullOrWhiteSpace(departureLocation) && departureLocation != "Any")
            {
                flightsQuery = flightsQuery.Where(f => f.Route.DepartureAirport.City == departureLocation);
            }

            // Filter by arrival location if provided
            if (!string.IsNullOrWhiteSpace(arrivalLocation) && arrivalLocation != "Any")
            {
                flightsQuery = flightsQuery.Where(f => f.Route.ArrivalAirport.City == arrivalLocation);
            }

            var flights = flightsQuery
                .Include(f => f.Route)
                .ThenInclude(r => r.DepartureAirport) // Include related properties of Route
                .Include(f => f.Route)
                .ThenInclude(r => r.ArrivalAirport) // Include ArrivalAirport in a separate Include
                .Include(f => f.Aircraft)
                .ThenInclude(a => a.AircraftModel) // Include AircraftModel
                .Include(f => f.Aircraft)
                .ThenInclude(a => a.Company) // Include Company in a separate Include
                .ToList();

            return View(flights);
        }


        //api endpoint
        [HttpGet]
        public IActionResult GetAvailableSeats(int flightId)
        {
            var flight = _context.Flights
                            .Include(f => f.Aircraft)
                            .ThenInclude(a => a.AircraftModel)
                            .FirstOrDefault(f => f.Id == flightId);

            if (flight == null)
            {
                return NotFound();
            }

            var totalSeats = flight.Aircraft.AircraftModel.Capacity;
            var bookedSeats = _context.Bookings.Count(b => b.FlightId == flightId);
            var availableSeats = totalSeats - bookedSeats;

            return Json(new { availableSeats });
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


        // POST: Buy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy([Bind("PurchaseTime,SeatNumber,FlightId,UserId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check if the seat is already booked on this flight
                var isSeatBooked = _context.Bookings.Any(b => b.FlightId == booking.FlightId && b.SeatNumber == booking.SeatNumber);

                if (!isSeatBooked)
                {
                    booking.ConvertTimesToUtc();
                    _context.Add(booking);
                    _context.SaveChanges();

                    TempData["msg"] = "Successfully booked a seat.";
                    return RedirectToAction("Details", new { id = booking.Id });
                }
                else
                {
                    TempData["err"] = "This seat is already booked.";
                }
            }
            else
            {
                TempData["err"] = "Failed to book a seat due to invalid model state.";
            }
            return View(booking);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Flight)
                .ThenInclude(f => f.Aircraft) // Include AircraftModel
                .ThenInclude(c => c.AircraftModel) // Include AircraftModel
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var bookedSeats = _context.Bookings.Where(b => b.FlightId == booking.FlightId).Select(b => b.SeatNumber).ToList();

            var viewModel = new BookingDetailsViewModel
            {
                Booking = booking,
                Flight = booking.Flight,
                User = booking.User,
                AircraftModel = booking.Flight.Aircraft.AircraftModel, // Include AircraftModel
                BookedSeats = bookedSeats // Include BookedSeats
            };

            return View(viewModel);
        }


        //api
        public async Task<IActionResult> GetSeatLayout(int id)
        {
            var aircraft = await _context.Flights
                .Include(f => f.Aircraft)
                .ThenInclude(a => a.AircraftModel)
                .Where(f => f.Id == id)
                .Select(f => f.Aircraft)
                .FirstOrDefaultAsync();

            var bookedSeats = await _context.Bookings
                .Where(b => b.FlightId == id)
                .Select(b => b.SeatNumber)
                .ToListAsync();

            if (aircraft == null)
            {
                return NotFound();
            }

            var viewModel = new AircraftDetailsViewModel
            {
                Aircraft = aircraft,
                AircraftModel = aircraft.AircraftModel,
                BookedSeats = bookedSeats, // Add this line
                                           // Other properties if needed
            };

            return PartialView("_SeatLayout", viewModel);
        }


        // GET: Home/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }



            var userId = int.Parse(userIdClaim.Value);
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Flight)
                .ThenInclude(f => f.Aircraft)
                .ThenInclude(a => a.AircraftModel)
                .Include(b => b.Flight)
                .ThenInclude(f => f.Aircraft)
                .ThenInclude(a => a.Company)
                .Include(b => b.Flight)
                .ThenInclude(f => f.Route)
                .ToListAsync();

            return View(bookings);
        }



    }
}
