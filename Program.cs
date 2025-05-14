using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_ST10150702.Data;
using PROG7311_POE_ST10150702.Models;
using PROG7311_POE_ST10150702.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Updated Identity configuration with proper role support
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // Add other identity options if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register the filter with proper dependencies
builder.Services.AddScoped<RoleRedirectFilter>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Middleware registration (simplified)
app.Use(async (context, next) =>
{
    // Skip filter for static files
    if (context.Request.Path.StartsWithSegments("/lib") ||
        context.Request.Path.StartsWithSegments("/css") ||
        context.Request.Path.StartsWithSegments("/js"))
    {
        await next();
        return;
    }

    var filter = context.RequestServices.GetRequiredService<RoleRedirectFilter>();
    var actionContext = new ActionContext
    {
        HttpContext = context,
        RouteData = context.GetRouteData(),
        ActionDescriptor = new ControllerActionDescriptor()
    };

    await filter.OnActionExecutionAsync(
        new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            context.RequestServices),
        async () => {
            await next();
            return new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), null);
        });
});

// Database seeding
await SeedDatabase(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

async Task SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Ensure all identity tables are properly created
        var roles = new[] { "Farmer", "Employee", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create admin user
        var adminEmail = "admin@farmcentral.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "Account",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@1234!");
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(adminUser, roles);

                // Create employee profile for admin
                if (!context.Employees.Any(e => e.UserId == adminUser.Id))
                {
                    context.Employees.Add(new Employee
                    {
                        FirstName = adminUser.FirstName,
                        LastName = adminUser.LastName,
                        UserId = adminUser.Id
                    });
                    await context.SaveChangesAsync();
                }
            }
        }

        // Verify all identity tables exist
        var identityTables = new[] { "AspNetUsers", "AspNetRoles", "AspNetUserRoles",
                                   "AspNetUserClaims", "AspNetUserLogins", "AspNetUserTokens",
                                   "AspNetRoleClaims" };

        foreach (var table in identityTables)
        {
            if (!context.Database.SqlQueryRaw<int>($"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}'").Any())
            {
                throw new Exception($"Identity table {table} not found after migration");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seeding failed");
    }
}