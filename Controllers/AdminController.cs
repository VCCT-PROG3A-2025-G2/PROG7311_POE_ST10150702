using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_ST10150702.Data;
using PROG7311_POE_ST10150702.Models;
using PROG7311_POE_ST10150702.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_POE_ST10150702.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AdminController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await GetDashboardViewModel();
            // Initialize the NewEmployee property to prevent null reference
            model.NewEmployee = new Employee();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([Bind("FirstName,LastName")] Employee employee, string email, string password)
        {
            try
            {
                // Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Email address is already in use.";
                    return RedirectToAction("Dashboard");
                }

                // Create Identity User
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Add to Employee role
                    await _userManager.AddToRoleAsync(user, "Employee");

                    // Create Employee record
                    employee.UserId = user.Id;
                    // Set the User navigation property to null - it will be handled by EF
                    employee.User = null;

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Employee {employee.FirstName} {employee.LastName} added successfully!";
                    return RedirectToAction("Dashboard");
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
                _logger.LogError(ex, "Error adding employee");
                TempData["ErrorMessage"] = "An unexpected error occurred while adding the employee: " + ex.Message;
            }

            return RedirectToAction("Dashboard");
        }

        private async Task<AdminDashboardViewModel> GetDashboardViewModel()
        {
            return new AdminDashboardViewModel
            {
                FarmerCount = await _context.Farmers.CountAsync(),
                EmployeeCount = await _context.Employees.CountAsync(),
                ProductCount = await _context.Products.CountAsync(),

                // Load farmers with their users
                Farmers = await _context.Farmers
                    .Include(f => f.User)
                    .ToListAsync(),

                // Load employees with their users
                Employees = await _context.Employees
                    .Include(e => e.User)
                    .ToListAsync(),

                // Load products WITHOUT Include for FarmerId
                Products = await _context.Products.ToListAsync()
            };
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found.";
                    return RedirectToAction("Dashboard");
                }

                var user = employee.User;

                // First remove employee (EF-tracked)
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                // Now delete Identity user (untracked)
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        TempData["ErrorMessage"] = $"Employee deleted, but failed to delete user: {errors}";
                        return RedirectToAction("Dashboard");
                    }
                }

                TempData["SuccessMessage"] = "Employee and user deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Dashboard");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            try
            {
                var farmer = await _context.Farmers
                    .Include(f => f.User)
                    .Include(f => f.Products)
                    .FirstOrDefaultAsync(f => f.FarmerId == id);

                if (farmer == null)
                {
                    TempData["ErrorMessage"] = "Farmer not found.";
                    return RedirectToAction("Dashboard");
                }

                var user = farmer.User;

                // Remove products
                if (farmer.Products != null && farmer.Products.Any())
                {
                    _context.Products.RemoveRange(farmer.Products);
                }

                // Remove the farmer (EF handles it)
                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync(); // Save changes BEFORE deleting user

                // Now delete the Identity user (AFTER SaveChanges)
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        TempData["ErrorMessage"] = $"Farmer deleted, but failed to delete user: {errors}";
                        return RedirectToAction("Dashboard");
                    }
                }

                TempData["SuccessMessage"] = "Farmer and user deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting farmer");
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Dashboard");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction("Dashboard");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Dashboard");
        }


    }
}