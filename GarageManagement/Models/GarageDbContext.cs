using GarageManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using DbContext = System.Data.Entity.DbContext;


namespace YourNamespace.Models
{
    public class GarageDbContext : DbContext
    {
        private const string V = "GarageDbContext";

        public GarageDbContext() : base(V) { }

        public System.Data.Entity.DbSet<Car> Cars { get; set; }
        public System.Data.Entity.DbSet<Maintenance> Maintenances { get; set; }
        public System.Data.Entity.DbSet<Owner> Owners { get; set; }
        public System.Data.Entity.DbSet<Garage> Garages { get; set; }
        public System.Data.Entity.DbSet<GarageCar> GarageCars { get; set; }
    }
}