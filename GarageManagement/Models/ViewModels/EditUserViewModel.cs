using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models.ViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Pole Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż {1} znaków")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Imię musi zaczynać się wielką literą i zawierać tylko litery")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż {1} znaków")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*(-[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*)?$", 
            ErrorMessage = "Nazwisko musi zaczynać się wielką literą i może zawierać tylko litery (dopuszczalny jest jeden myślnik dla nazwisk dwuczłonowych)")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole Email jest wymagane")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
        [StringLength(256, ErrorMessage = "Email nie może być dłuższy niż {1} znaków")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
        [RegularExpression(@"^\+?[0-9]{9,12}$", ErrorMessage = "Numer telefonu powinien zawierać 9-12 cyfr (może zaczynać się od +)")]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Aktualne hasło")]
        public string CurrentPassword { get; set; }

        [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków i maksymalnie {1} znaków.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", 
            ErrorMessage = "Hasło musi zawierać co najmniej jedną małą literę, wielką literę, cyfrę i znak specjalny")]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są identyczne")]
        public string ConfirmPassword { get; set; }
    }
} 