using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_ST10150702.Models;

namespace PROG7311_POE_ST10150702.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
