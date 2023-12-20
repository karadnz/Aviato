using System.Data;
using flightMVC.Models;
using flightMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using Route = flightMVC.Models.Route;

namespace flightMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RouteController : Controller
    {
        private readonly DataContext _context;

        public RouteController(DataContext context)
        {
            _context = context;
        }

        // GET: Route
        public IActionResult Index()
        {
            var routes = _context.Routes
                .Include(r => r.DepartureAirport)
                .Include(r => r.ArrivalAirport)
                .ToList();
            return View(routes);
        }

        // GET: Route/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.DepartureAirport)
                .Include(r => r.ArrivalAirport)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }


        // GET: Route/Create
        public IActionResult Create()
        {
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name");
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name");
            return View();
        }

        // POST: Route/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,DepartureAirportId,ArrivalAirportId,RouteCode")] Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                _context.SaveChanges();
                TempData["msj"] = "Route has been added";
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name", route.DepartureAirportId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name", route.ArrivalAirportId);
            return View(route);
        }



        // GET: Route/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = _context.Routes.Find(id);
            if (route == null)
            {
                return NotFound();
            }

            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name", route.DepartureAirportId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name", route.ArrivalAirportId);
            return View(route);
        }

        // POST: Route/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,DepartureAirportId,ArrivalAirportId,RouteCode")] Route route)
        {
            if (id != route.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["msj"] = "Route has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }

     

        [HttpPost]
        public async Task<JsonResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return Json(new { success = false, message = "Route not found" });
            }

            if (_context.Flights.Any(r => r.RouteId == id))
            {
                return Json(new { success = false, message = "Cannot delete: This route is used in flights" });
            }
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Route deleted successfully" });
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.Id == id);
        }
    }
}
