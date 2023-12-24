using flightMVC.Models;
using flightMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class BookingController : Controller
    {
        private readonly DataContext _context;

        public BookingController(DataContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public IActionResult Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.User)
                .ToList();
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new BookingDetailsViewModel
            {
                Booking = booking,
                Flight = booking.Flight,
                User = booking.User
            };

            return View(viewModel);
        }


        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNumber");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "username");
            return View();
        }

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


        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,PurchaseTime,SeatNumber,FlightId,UserId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.ConvertTimesToUtc();
                _context.Add(booking);
                _context.SaveChanges();
                TempData["msj"] = "Booking created successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNumber", booking.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "username", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNumber", booking.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "username", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,PurchaseTime,SeatNumber,FlightId,UserId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    booking.ConvertTimesToUtc();
                    _context.Update(booking);
                    _context.SaveChanges();
                    TempData["msj"] = "Booking updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNumber", booking.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "username", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost]
        public async Task<JsonResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return Json(new { success = false, message = "Booking not found" });
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Booking deleted successfully" });
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
