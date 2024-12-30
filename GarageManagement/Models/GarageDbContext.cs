using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Models
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<GarageCar> GarageCars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().ToTable("Car");
            modelBuilder.Entity<Owner>().ToTable("Owner");
            modelBuilder.Entity<Maintenance>().ToTable("Maintenance");
            modelBuilder.Entity<Garage>().ToTable("Garage");
            modelBuilder.Entity<GarageCar>().ToTable("GarageCar");

            modelBuilder.Entity<GarageCar>()
                .HasKey(gc => new { gc.GarageId, gc.CarId });

            modelBuilder.Entity<GarageCar>()
                .HasOne(gc => gc.Garage)
                .WithMany(g => g.GarageCars)
                .HasForeignKey(gc => gc.GarageId);

            modelBuilder.Entity<GarageCar>()
                .HasOne(gc => gc.Car)
                .WithMany(c => c.GarageCars)
                .HasForeignKey(gc => gc.CarId);

            base.OnModelCreating(modelBuilder);
        }
    }
}