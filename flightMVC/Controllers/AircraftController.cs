using flightMVC.Models;
using flightMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    public class AircraftController : Controller
    {
        private readonly DataContext _context;

        public AircraftController(DataContext context)
        {
            _context = context;
        }

        // GET: Aircraft
        public IActionResult Index()
        {
            var aircrafts = _context.Aircrafts.Include(a => a.AircraftModel).Include(a => a.Company).ToList();
            return View(aircrafts);
        }

        // GET: Aircraft/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aircraft = await _context.Aircrafts
                .Include(a => a.AircraftModel)
                .Include(a => a.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aircraft == null)
            {
                return NotFound();
            }

            var viewModel = new AircraftDetailsViewModel
            {
                Aircraft = aircraft,
                AircraftModel = aircraft.AircraftModel,
                Company = aircraft.Company
            };

            return View(viewModel);
        }

        // GET: Aircraft/Create
        public IActionResult Create()
        {
            ViewData["AircraftModelId"] = new SelectList(_context.AircraftModels, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Aircraft/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,AircraftName,AircraftModelId,CompanyId")] Aircraft aircraft)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aircraft);
                _context.SaveChanges();
                TempData["msj"] = aircraft.AircraftName + " has been added";
                return RedirectToAction(nameof(Index));
            }

            ViewData["AircraftModelId"] = new SelectList(_context.AircraftModels, "Id", "Name", aircraft.AircraftModelId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", aircraft.CompanyId);
            return View(aircraft);
        }

        // GET: Aircraft/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aircraft = _context.Aircrafts.Find(id);
            if (aircraft == null)
            {
                return NotFound();
            }

            ViewData["AircraftModelId"] = new SelectList(_context.AircraftModels, "Id", "Name", aircraft.AircraftModelId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", aircraft.CompanyId);
            return View(aircraft);
        }

        // POST: Aircraft/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,AircraftName,AircraftModelId,CompanyId")] Aircraft aircraft)
        {
            if (id != aircraft.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aircraft);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AircraftExists(aircraft.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["msj"] = aircraft.AircraftName + " has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(aircraft);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAircraft(int id)
        {
            var aircraft = await _context.Aircrafts.FindAsync(id);
            if (aircraft == null)
            {
                return Json(new { success = false, message = "Aircraft not found" });
            }

            _context.Aircrafts.Remove(aircraft);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Aircraft deleted successfully" });
        }


        private bool AircraftExists(int id)
        {
            return _context.Aircrafts.Any(e => e.Id == id);
        }
    }
}
