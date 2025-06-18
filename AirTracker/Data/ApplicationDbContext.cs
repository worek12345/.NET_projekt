using AirTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Dodajemy DbSet dla lokalizacji
        public DbSet<Location> Locations { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SensorCategory> SensorCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many Sensor <-> Category
            modelBuilder.Entity<SensorCategory>()
                .HasKey(sc => new { sc.SensorId, sc.CategoryId });
            modelBuilder.Entity<SensorCategory>()
                .HasOne(sc => sc.Sensor)
                .WithMany(s => s.SensorCategories)
                .HasForeignKey(sc => sc.SensorId);
            modelBuilder.Entity<SensorCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SensorCategories)
                .HasForeignKey(sc => sc.CategoryId);

                        // One-to-many Location -> Sensors
            modelBuilder.Entity<Sensor>()
                            .HasOne(s => s.Location)
                            .WithMany(l => l.Sensors)
                            .HasForeignKey(s => s.LocationId)
                            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
