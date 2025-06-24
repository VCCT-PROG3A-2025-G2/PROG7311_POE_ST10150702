using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_ST10150702.Data;
using PROG7311_POE_ST10150702.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_ST10150702.ViewModels;

/*
    ===== HomeController =====

    Handles the main application views and user-specific dashboards.

    Actions:
    - Index():            Displays the home page.
    - Privacy():          Displays the privacy policy page.
    - FarmerView():       Displays the dashboard for Farmers with their products.
    - AddProduct(Product):Allows Farmers to add new products.
    - EmployeeView():     Displays the dashboard for Employees with farmer and product management.
    - AddFarmer(...):     Allows Employees to add new Farmer users and profiles.
    - Error():            Displays the error page.

    Role-based access:
    - FarmerView and AddProduct restricted to Farmers.
    - EmployeeView and AddFarmer restricted to Employees.

    Dependencies:
    - ILogger for logging
    - ApplicationDbContext for data access
    - UserManager for user management
*/


namespace PROG7311_POE_ST10150702.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> FarmerView()
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (farmer == null) return NotFound();

            // Load products directly without navigation property
            var products = await _context.Products
                .Where(p => p.FarmerId == farmer.FarmerId)
                .OrderByDescending(p => p.ProductionDate)
                .ToListAsync();

            ViewBag.FarmerId = farmer.FarmerId;
            ViewBag.FarmerFirstName = farmer.FirstName;
            ViewBag.FarmerProducts = products;

            return View(new Product { FarmerId = farmer.FarmerId });
        }


        [HttpPost]
        [Authorize(Roles = "Farmer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verify the farmer exists
                    var farmerExists = await _context.Farmers
                        .AnyAsync(f => f.FarmerId == product.FarmerId);

                    if (!farmerExists)
                    {
                        ModelState.AddModelError("", "Invalid farmer specified");
                        return View("FarmerView", product);
                    }

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Product '{product.Name}' added successfully!";
                    return RedirectToAction("FarmerView");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding product");
                    ModelState.AddModelError("", "Error saving product. Please try again.");
                }
            }

            // Repopulate ViewBag if returning to view
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
            ViewBag.FarmerId = farmer?.FarmerId;
            ViewBag.FarmerFirstName = farmer?.FirstName;

            return View("FarmerView", product);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EmployeeView()
        {
            // Set the navbar class for the Employee view
            ViewData["NavbarClass"] = "navbar-custom";

            var model = new EmployeeDashboardViewModel
            {
                Farmers = await _context.Farmers
                    .Include(f => f.User)
                    .ToListAsync(),
                Products = await _context.Products
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(
            string FirstName,
            string LastName,
            string Email,
            string Password,
            string Region,
            bool AcceptedPOPPIA)
        {
            try
            {
                // Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(Email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Email address is already in use.";
                    return RedirectToAction("EmployeeView");
                }

                // Create Identity User
                var user = new ApplicationUser
                {
                    UserName = Email,
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName
                };

                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    // Add to Farmer role
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    // Create Farmer record
                    var farmer = new Farmer
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Region = Region,
                        AcceptedPOPPIA = AcceptedPOPPIA,
                        UserId = user.Id
                    };

                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Farmer {FirstName} {LastName} added successfully!";
                    return RedirectToAction("EmployeeView");
                }
                else
                {
                    // Handle identity errors
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["ErrorMessage"] = $"Failed to create user: {errors}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding farmer");
                TempData["ErrorMessage"] = "An unexpected error occurred while adding the farmer.";
            }

            return RedirectToAction("EmployeeView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}