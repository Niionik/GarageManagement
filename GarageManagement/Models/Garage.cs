using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class Garage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lokalizacja jest wymagana")]
        [StringLength(200)]
        public string Location { get; set; }

        public string? OwnerId { get; set; }
        public virtual Owner? Owner { get; set; }

        public virtual ICollection<GarageCar>? GarageCars { get; set; }
    }
}
