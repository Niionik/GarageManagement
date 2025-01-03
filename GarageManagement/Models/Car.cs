using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Marka jest wymagane")]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Pole Model jest wymagane")]
        [StringLength(50)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Pole Rok produkcji jest wymagane")]
        [Range(1900, 2024, ErrorMessage = "Rok musi być między 1900 a 2024")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Pole Przebieg jest wymagane")]
        [Range(0, int.MaxValue, ErrorMessage = "Przebieg nie może być ujemny")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Pole Status jest wymagane")]
        [StringLength(20)]
        public string Status { get; set; }

        public string? WheelModel { get; set; }
        public string? TireSize { get; set; }
        public string? TireBrand { get; set; }
        public DateTime? LastOilChange { get; set; }
        public DateTime? LastTimingBeltChange { get; set; }

        public virtual ICollection<Maintenance>? Maintenances { get; set; }
        public virtual ICollection<GarageCar>? GarageCars { get; set; }
    }
}
