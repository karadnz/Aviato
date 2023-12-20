using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using flightMVC.Models;
using System.Threading.Tasks;
using WebApi.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace flightMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.username == username && u.password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("FullName", user.Name + " " + user.Surname),
                    new Claim(ClaimTypes.Role, user.Role),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Configure additional properties as needed
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home"); // Redirect to a secure page
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "User"; // Assign "User" role
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["msj"] = user.Name + " has been registered";
                return RedirectToAction(nameof(Index)); // Redirect to login page after registration
            }
            TempData["msj"] =" failed to register";
            return View(user);
        }

        // POST: Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
