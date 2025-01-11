using GarageManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GarageManagement.Models
{
    public class GarageDbContext : IdentityDbContext<Owner>
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options)
            : base(options)
        {
        }

        public DbSet<Garage> Garages { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<GarageCar> GarageCars { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapowanie nazw tabel
            modelBuilder.Entity<Garage>().ToTable("Garage");
            modelBuilder.Entity<Car>().ToTable("Car");
            modelBuilder.Entity<GarageCar>().ToTable("GarageCars");
            modelBuilder.Entity<Maintenance>().ToTable("Maintenance");

            modelBuilder.Entity<Garage>(entity =>
            {
                entity.Property(e => e.OwnerId)
                    .HasColumnType("nvarchar(450)");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Garages)
                    .HasForeignKey(d => d.OwnerId);
            });

            modelBuilder.Entity<GarageCar>(entity =>
            {
                entity.HasKey(gc => new { gc.GarageId, gc.CarId });

                entity.HasOne(gc => gc.Garage)
                    .WithMany(g => g.GarageCars)
                    .HasForeignKey(gc => gc.GarageId);

                entity.HasOne(gc => gc.Car)
                    .WithMany(c => c.GarageCars)
                    .HasForeignKey(gc => gc.CarId);
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.Owner)
                    .WithMany()
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.CarId);
            });
        }
    }
}
