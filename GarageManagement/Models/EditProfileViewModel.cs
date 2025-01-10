using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
    [StringLength(255)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
    public string Email { get; set; }
}