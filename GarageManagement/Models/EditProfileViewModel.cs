using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "Nazwa u¿ytkownika jest wymagana.")]
    [StringLength(255)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
    [EmailAddress(ErrorMessage = "Nieprawid³owy format adresu e-mail.")]
    public string Email { get; set; }
}