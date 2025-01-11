namespace GarageManagement.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string? OwnerId { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<GarageCar>? GarageCars { get; set; }
    }
}
