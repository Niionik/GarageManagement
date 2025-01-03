using GarageManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class GarageCar
    {
       
        public int GarageId { get; set; }

        public int CarId { get; set; }
        public required Garage Garage { get; set; }
        public required Car Car { get; set; }
    }
}

