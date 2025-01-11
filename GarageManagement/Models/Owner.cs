using Microsoft.AspNetCore.Identity;

namespace GarageManagement.Models
{
    public class Owner : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<Garage>? Garages { get; set; }
        public virtual ICollection<Car>? Cars { get; set; }
        public virtual ICollection<Maintenance>? Maintenances { get; set; }
    }
}
