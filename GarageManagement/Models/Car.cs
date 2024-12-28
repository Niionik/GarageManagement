namespace GarageManagement.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Status { get; set; } // Active, Repair, Broken
        public string WheelModel { get; set; }


        public string TireSize { get; set; }
        public string TireBrand { get; set; }
        public DateTime? LastOilChange { get; set; }
        public DateTime? LastTimingBeltChange { get; set; }

        public ICollection<Maintenance> Maintenances { get; set; }
        public ICollection<GarageCar> GarageCars { get; set; }
    }
}
