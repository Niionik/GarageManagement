﻿namespace GarageManagement.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public Car Car { get; set; }
    }
}
