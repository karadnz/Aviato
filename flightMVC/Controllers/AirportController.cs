using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using flightMVC.Models;
using flightMVC.ViewModels;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    public class AirportController : Controller
    {
        private readonly DataContext _context;

        public AirportController(DataContext context)
        {
            _context = context;
        }

        // GET: Airport
        public async Task<IActionResult> Index()
        {
            var airports = await _context.Airports.ToListAsync();
            return View(airports);
        }

        // GET: Airport/Details/5
        // GET: Airport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport2 = await _context.Airports
                .Include(a => a.DepartureRoutes)
                .Include(a => a.ArrivalRoutes)
                .FirstOrDefaultAsync(m => m.Id == id);

            var airport = await _context.Airports
    .Include(a => a.DepartureRoutes)
        .ThenInclude(dr => dr.ArrivalAirport) // Eager loading of related data
    .Include(a => a.ArrivalRoutes)
        .ThenInclude(ar => ar.DepartureAirport) // Eager loading of related data
    .FirstOrDefaultAsync(m => m.Id == id);


            if (airport == null)
            {
                return NotFound();
            }

            var viewModel = new AirportDetailsViewModel
            {
                Airport = airport,
                DepartureRoutes = airport.DepartureRoutes.ToDictionary(
                    route => route,
                    route => route.ArrivalAirportId // or any other relevant data
                ),
                ArrivalRoutes = airport.ArrivalRoutes.ToDictionary(
                    route => route,
                    route => route.DepartureAirportId // or any other relevant data
                )
            };

            return View(viewModel);
        }

        // GET: Airport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        // GET: Airport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports.FindAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }

        // POST: Airport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City")] Airport airport)
        {
            if (id != airport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirportExists(airport.Id))
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
            return View(airport);
        }

        // POST: Airport/Delete/5
        //sil
        [HttpPost]
        public async Task<JsonResult> DeleteAirport(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport == null)
            {
                return Json(new { success = false, message = "Airport not found" });
            }

            // Optional: Check for dependencies before deleting
            if (_context.Routes.Any(r => r.DepartureAirportId == id || r.ArrivalAirportId == id))
            {
                return Json(new { success = false, message = "Cannot delete: This airport is used in routes" });
            }

            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Airport deleted successfully" });
        }


        private bool AirportExists(int id)
        {
            return _context.Airports.Any(e => e.Id == id);
        }
    }
}
