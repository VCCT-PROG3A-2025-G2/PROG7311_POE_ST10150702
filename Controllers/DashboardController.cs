// Controllers/DashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Farmer")]
public class FarmerDashboardController : Controller
{
    [Authorize(Roles = "Farmer")]
    public IActionResult FarmerView()
    {
        Console.WriteLine("DEBUG: FarmerView action executing"); // Check console
        Console.WriteLine($"View Path: {Path.GetFullPath("Views/Dashboard/FarmerView.cshtml")}"); // Verify path
        return View();
    }
}

[Authorize(Roles = "Employee")]
public class EmployeeDashboardController : Controller
{
    public IActionResult EmployeeView()
    {
        return View();
    }
}