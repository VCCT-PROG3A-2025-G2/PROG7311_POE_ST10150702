using Microsoft.AspNetCore.Identity;
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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename Identity tables to use AspNet prefix
            builder.Entity<ApplicationUser>(entity => {
                entity.ToTable("AspNetUsers");
            });
            builder.Entity<IdentityRole>(entity => {
                entity.ToTable("AspNetRoles");
            });
            builder.Entity<IdentityUserRole<string>>(entity => {
                entity.ToTable("AspNetUserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity => {
                entity.ToTable("AspNetUserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity => {
                entity.ToTable("AspNetUserLogins");
            });
            builder.Entity<IdentityUserToken<string>>(entity => {
                entity.ToTable("AspNetUserTokens");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity => {
                entity.ToTable("AspNetRoleClaims");
            });

            // Configure Farmer entity - modified to ensure proper identity increment
            builder.Entity<Farmer>(entity => {
                entity.Property(e => e.FarmerId).UseIdentityColumn(); 
                entity.HasOne(f => f.User)
                      .WithMany()
                      .HasForeignKey(f => f.UserId);
            });

            // Configure Product entity
            builder.Entity<Product>(entity => {
                entity.Property(e => e.ProductId).UseIdentityColumn(); 
                entity.HasOne<Farmer>()
                      .WithMany(f => f.Products)
                      .HasForeignKey(p => p.FarmerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Employee entity
            builder.Entity<Employee>(entity => {
                entity.Property(e => e.EmployeeId).UseIdentityColumn();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId);
            });
        }
    }
}