using System.Collections.Generic;

namespace GarageManagement.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalGarages { get; set; }
        public int TotalCars { get; set; }
        public int TotalMaintenances { get; set; }
        public List<Owner> RecentUsers { get; set; }
        public List<Maintenance> RecentMaintenances { get; set; }
    }

    public class AdminReportsViewModel
    {
        public List<UserStatistics> UserStatistics { get; set; }
        public List<MaintenanceStatistics> MaintenanceStatistics { get; set; }
    }

    public class UserStatistics
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public int GaragesCount { get; set; }
        public int CarsCount { get; set; }
        public int MaintenancesCount { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
    }

    public class MaintenanceStatistics
    {
        public int Month { get; set; }
        public int Count { get; set; }
        public decimal TotalCost { get; set; }
    }
} 