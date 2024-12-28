using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Models
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options) { }

        public DbSet<Car> Car { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<GarageCar> GarageCars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GarageCar>()
                .HasKey(gc => new { gc.GarageId, gc.CarId });

            base.OnModelCreating(modelBuilder);
        }
    }
}