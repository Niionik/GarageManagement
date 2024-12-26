namespace GarageManagement.Models
{
    public class GarageCar
    {
        public int GarageId { get; set; }
        public int CarId { get; set; }

        public Garage Garage { get; set; }
        public Car Car { get; set; }
    }
}
