using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
        public string? Email { get; set; }
    }
}