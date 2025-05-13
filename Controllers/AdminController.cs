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
            model.NewEmployee = new Employee();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([Bind("FirstName,LastName")] Employee employee, string email, string password)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Email address is already in use.";
                    return RedirectToAction("Dashboard");
                }

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
                    await _userManager.AddToRoleAsync(user, "Employee");
                    employee.UserId = user.Id;
                    employee.User = null;

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Employee {employee.FirstName} {employee.LastName} added successfully!";
                    return RedirectToAction("Dashboard");
                }
                else
                {
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
                Farmers = await _context.Farmers.Include(f => f.User).ToListAsync(),
                Employees = await _context.Employees.Include(e => e.User).ToListAsync(),
                Products = await _context.Products.ToListAsync()
            };
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (employee == null)
                {
                    return RedirectToAction("Dashboard");
                }

                if (employee.User != null)
                {
                    await _userManager.DeleteAsync(employee.User);
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Employee deleted successfully!";
            }
            catch
            {
                await transaction.RollbackAsync();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var farmer = await _context.Farmers
                    .Include(f => f.User)
                    .Include(f => f.Products)
                    .FirstOrDefaultAsync(f => f.FarmerId == id);

                if (farmer == null)
                {
                    return RedirectToAction("Dashboard");
                }

                if (farmer.Products?.Any() == true)
                {
                    _context.Products.RemoveRange(farmer.Products);
                    await _context.SaveChangesAsync();
                }

                if (farmer.User != null)
                {
                    await _userManager.DeleteAsync(farmer.User);
                }

                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Farmer and all associated data deleted successfully!";
            }
            catch
            {
                await transaction.RollbackAsync();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product deleted successfully!";
                }
            }
            catch { }

            return RedirectToAction("Dashboard");
        }
    }
}