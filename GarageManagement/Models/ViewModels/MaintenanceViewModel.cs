using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models.ViewModels
{
    public class MaintenanceViewModel
    {
        [Required(ErrorMessage = "Data jest wymagana")]
        [Display(Name = "Data naprawy")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany")]
        [Display(Name = "Opis naprawy")]
        [StringLength(500, ErrorMessage = "Opis nie może być dłuższy niż 500 znaków")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Koszt jest wymagany")]
        [Display(Name = "Koszt naprawy")]
        [Range(0, 1000000, ErrorMessage = "Koszt musi być między 0 a 1,000,000")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        public int CarId { get; set; }
    }
} 