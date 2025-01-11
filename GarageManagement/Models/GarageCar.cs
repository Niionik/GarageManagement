using GarageManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Models
{
    public class GarageCar
    {
        public int GarageId { get; set; }
        public int CarId { get; set; }
        
        public virtual Garage Garage { get; set; }
        public virtual Car Car { get; set; }
    }
}

