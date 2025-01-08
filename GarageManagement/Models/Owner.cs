using Microsoft.AspNetCore.Identity;

namespace GarageManagement.Models
{
    public class Owner:IdentityUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Garage> Garages { get; set; }
    }
}
