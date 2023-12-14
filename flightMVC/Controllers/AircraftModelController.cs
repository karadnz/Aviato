using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using flightMVC.Models;
using WebApi.Helpers;
using flightMVC.ViewModels;

namespace flightMVC.Controllers
{
    public class AircraftModelController : Controller
    {
        private readonly DataContext _context;

        public AircraftModelController(DataContext context)
        {
            _context = context;
        }

        // GET: AircraftModel
        public IActionResult Index()
        {
            var aircraftModels = _context.AircraftModels.ToList();
            return View(aircraftModels);
        }

        // GET: AircraftModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aircraftModel = await _context.AircraftModels
                .Include(am => am.Aircrafts)
                .ThenInclude(a => a.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aircraftModel == null)
            {
                return NotFound();
            }

            var viewModel = new AircraftModelDetailsViewModel
            {
                AircraftModel = aircraftModel,
                CompaniesWithModel = aircraftModel.Aircrafts
                    .GroupBy(a => a.Company)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(viewModel);
        }

        // GET: AircraftModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AircraftModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity")] AircraftModel aircraftModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aircraftModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aircraftModel);
        }

        // GET: AircraftModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aircraftModel = await _context.AircraftModels.FindAsync(id);
            if (aircraftModel == null)
            {
                return NotFound();
            }
            return View(aircraftModel);
        }

        // POST: AircraftModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity")] AircraftModel aircraftModel)
        {
            if (id != aircraftModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aircraftModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AircraftModelExists(aircraftModel.Id))
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
            return View(aircraftModel);
        }

        // POST: AircraftModel/Delete/5
        [HttpPost]
        public async Task<JsonResult> DeleteAircraftModel(int id)
        {
            var aircraftModel = await _context.AircraftModels.FindAsync(id);
            if (aircraftModel == null)
            {
                return Json(new { success = false, message = "Aircraft model not found" });
            }

            // Optional: Check for dependencies before deleting
            if (_context.Aircrafts.Any(a => a.AircraftModelId == id))
            {
                return Json(new { success = false, message = "Cannot delete: This model is in use by aircrafts" });
            }

            _context.AircraftModels.Remove(aircraftModel);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Aircraft model deleted successfully" });
        }


        private bool AircraftModelExists(int id)
        {
            return _context.AircraftModels.Any(e => e.Id == id);
        }
    }
}
