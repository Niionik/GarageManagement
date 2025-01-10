using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Nazwa u¿ytkownika jest wymagana.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawid³owy adres e-mail.")]
        public string? Email { get; set; }
    }
}