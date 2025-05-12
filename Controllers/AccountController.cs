using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_ST10150702.Data;
using PROG7311_POE_ST10150702.Models;
using PROG7311_POE_ST10150702.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PROG7311_POE_ST10150702.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    // Debug output (check server logs)
                    Console.WriteLine($"User roles: {string.Join(", ", roles)}");

                    // Role priority: Admin > Employee > Farmer
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Dashboard", "Admin"); // Explicit Admin redirect
                    }
                    else if (roles.Contains("Employee"))
                    {
                        return RedirectToAction("EmployeeView", "Home");
                    }
                    else if (roles.Contains("Farmer"))
                    {
                        return RedirectToAction("FarmerView", "Home");
                    }

                    // Fallback for users with no roles
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign the "Farmer" role
                await _userManager.AddToRoleAsync(user, "Farmer");

                // Create farmer profile
                var farmer = new Farmer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Region = model.Region,
                    AcceptedPOPPIA = model.AcceptedPOPPIA,
                    UserId = user.Id
                };
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("FarmerView", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}