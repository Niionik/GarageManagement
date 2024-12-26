namespace GarageManagement.Models
{
    public class Garage
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public ICollection<GarageCar> GarageCars { get; set; }
    }
}
