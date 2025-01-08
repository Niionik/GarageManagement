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

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Maintenance>()
                .Property(m => m.Cost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Owner>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(o => o.Id)
                .IsRequired();

            modelBuilder.Entity<Car>().ToTable("Car");
            modelBuilder.Entity<Maintenance>().ToTable("Maintenance");
            modelBuilder.Entity<Garage>().ToTable("Garage");
            modelBuilder.Entity<GarageCar>().ToTable("GarageCars");

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

            modelBuilder.ApplyConfiguration(new GarageEntityConfiguration());
        }

        public class GarageEntityConfiguration : IEntityTypeConfiguration<Garage>
        {
            public void Configure(EntityTypeBuilder<Garage> builder)
            {
                builder.Property(x => x.Name)
                    .HasMaxLength(255)
                    .IsRequired();

                builder.Property(x => x.Location)
                    .HasMaxLength(255);
            }
        }
    }
}
