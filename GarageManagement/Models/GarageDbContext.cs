using GarageManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace YourNamespace.Models
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options) { }

        public DbSet<Car>? Cars { get; set; }
        public DbSet<Maintenance>? Maintenances { get; set; }
        public DbSet<Owner>? Owners { get; set; }
        public DbSet<Garage>? Garages { get; set; }
        public DbSet<GarageCar>? GarageCars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GarageEntityConfiguration());
        }
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
