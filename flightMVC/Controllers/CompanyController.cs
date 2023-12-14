using flightMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;

namespace flightMVC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly DataContext _context;

        public CompanyController(DataContext context)
        {
            _context = context;
        }

        // GET: Company
        public IActionResult Index()
        {
            var companies = _context.Companies.ToList();
            return View(companies);
        }

        // GET: Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.Aircrafts)
                    .ThenInclude(a => a.AircraftModel) // Assuming you want to show AircraftModel details as well
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                _context.SaveChanges();
                TempData["msj"] = company.Name + " has been added";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["msj"] = "Fail to add";
            return View(company);
        }

        // GET: Company/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,City")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["msj"] = company.Name + " has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return Json(new { success = false, message = "Company not found" });
            }

            // Check if the company has any associated aircraft
            if (_context.Aircrafts.Any(a => a.CompanyId == id))
            {
                return Json(new { success = false, message = "Cannot delete: This company has associated aircraft" });
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Company deleted successfully" });
        }


        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
