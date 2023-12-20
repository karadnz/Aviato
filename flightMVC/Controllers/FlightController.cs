using flightMVC.Models;
using flightMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightController : Controller
    {
        private readonly DataContext _context;

        public FlightController(DataContext context)
        {
            _context = context;
        }

        // GET: Flights
        public IActionResult Index()
        {
            var flights = _context.Flights
                .Include(f => f.Route)
                .Include(f => f.Aircraft)
                .ToList();
            return View(flights);
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Route)
                    .ThenInclude(r => r.DepartureAirport)
                .Include(f => f.Route)
                    .ThenInclude(r => r.ArrivalAirport)
                .Include(f => f.Aircraft)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            var viewModel = new FlightDetailsViewModel
            {
                Flight = flight,
                Route = flight.Route,
                Aircraft = flight.Aircraft,
                DepartureAirport = flight.Route.DepartureAirport,
                ArrivalAirport = flight.Route.ArrivalAirport,
                Duration = flight.ArrivalTime - flight.DepartureTime
            };

            return View(viewModel);
        }


        // GET: Flights/Create
        public IActionResult Create()
        {
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteCode"); // Assuming Routes have a Description field
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "AircraftName");
            return View();
        }

        // POST: Flights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FlightNumber,DepartureTime,ArrivalTime,RouteId,AircraftId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                flight.ConvertTimesToUtc();
                _context.Add(flight);
                _context.SaveChanges();
                TempData["msj"] = "Flight " + flight.FlightNumber + " has been added";
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteCode", flight.RouteId);
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "AircraftName", flight.AircraftId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteCode", flight.RouteId);
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "AircraftName", flight.AircraftId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FlightNumber,DepartureTime,ArrivalTime,RouteId,AircraftId")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    flight.ConvertTimesToUtc();
                    _context.Update(flight);
                    _context.SaveChanges();
                    TempData["msj"] = "Flight " + flight.FlightNumber + " has been updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
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
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteCode", flight.RouteId);
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "AircraftName", flight.AircraftId);
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost]
        public async Task<JsonResult> DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return Json(new { success = false, message = "Flight not found" });
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Flight deleted successfully" });
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}
