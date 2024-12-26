using GarageManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace YourNamespace.Models
{
    public class GarageDbContext : DbContext
    {
        private const string V = "GarageDbContext";

        public GarageDbContext() : base(V) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<GarageCar> GarageCars { get; set; }
    }
}