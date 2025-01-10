using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class Garage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa garażu jest wymagana.")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lokalizacja garażu jest wymagana.")]
        [StringLength(255)]
        public string Location { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public ICollection<GarageCar> GarageCars { get; set; }
    }
}
