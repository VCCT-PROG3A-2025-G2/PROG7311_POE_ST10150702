using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using PROG7311_POE_ST10150702.Data;
using System.Threading.Tasks;
using PROG7311_POE_ST10150702.Models;

namespace PROG7311_POE_ST10150702.Filters
{
    public class RoleRedirectFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleRedirectFilter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // Skip authentication checks for these paths
            var excludedPaths = new[] { "Account", "Home", "Error", "Admin" };
            if (excludedPaths.Contains(controller))
            {
                await next();
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new
                {
                    ReturnUrl = context.HttpContext.Request.Path
                });
                return;
            }

            var user = await _userManager.GetUserAsync(context.HttpContext.User);

            // Admin redirection (highest priority)
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                if (controller != "Admin" || action != "Dashboard")
                {
                    context.Result = new RedirectToActionResult("Dashboard", "Admin", null);
                    return;
                }
            }
            // Employee redirection
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                if (controller != "Dashboard" || action != "EmployeeView")
                {
                    context.Result = new RedirectToActionResult("EmployeeView", "Dashboard", null);
                    return;
                }
            }
            // Farmer redirection
            else if (await _userManager.IsInRoleAsync(user, "Farmer"))
            {
                if (controller != "Dashboard" || action != "FarmerView")
                {
                    context.Result = new RedirectToActionResult("FarmerView", "Dashboard", null);
                    return;
                }
            }

            await next();
        }
    }
}