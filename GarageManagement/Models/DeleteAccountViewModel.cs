using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class DeleteAccountViewModel
    {
        [Required(ErrorMessage = "Podanie hasła jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}