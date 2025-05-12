using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_ST10150702.Data;
using PROG7311_POE_ST10150702.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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

            return View("FarmerView", product);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeView()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}