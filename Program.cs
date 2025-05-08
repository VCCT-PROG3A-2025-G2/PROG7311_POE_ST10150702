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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
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

        await context.Database.MigrateAsync();

        var roles = new[] { "Farmer", "Employee", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

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

                // Only create employee profile if needed
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
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seeding failed");
    }
}