using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        
        [Required]
        [Range(0, 1000000)]
        public decimal Cost { get; set; }

        public string? OwnerId { get; set; }
        public virtual Owner? Owner { get; set; }
    }
}
