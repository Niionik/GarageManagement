using GarageManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class GarageCar
    {
        [Key]
        public int GarageId { get; set; }

        [Key]
        public int CarId { get; set; }
        public Garage Garage { get; set; }
        public Car Car { get; set; }
    }
}

