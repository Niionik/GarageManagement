namespace GarageManagement.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public string? OwnerId { get; set; }

        public virtual Car? Car { get; set; }
        public virtual Owner? Owner { get; set; }
    }
}
