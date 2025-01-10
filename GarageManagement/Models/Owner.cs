using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class Owner:IdentityUser
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(255)]
        public string? LastName { get; set; }

        public ICollection<Garage>? Garages { get; set; }
    }
}
