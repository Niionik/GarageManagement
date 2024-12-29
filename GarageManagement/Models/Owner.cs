namespace GarageManagement.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public ICollection<Garage>? Garages { get; set; }
    }
}
