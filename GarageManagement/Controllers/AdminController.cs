using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using GarageManagement.Models.ViewModels;
using System.Security.Claims;

namespace GarageManagement.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Owner> _userManager;
        private readonly GarageDbContext _context;

        public AdminController(UserManager<Owner> userManager, GarageDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalGarages = await _context.Garages.CountAsync(),
                TotalCars = await _context.Cars.CountAsync(),
                TotalMaintenances = await _context.Maintenances.CountAsync(),
                RecentUsers = await _userManager.Users
                    .OrderByDescending(u => u.Id)
                    .Take(5)
                    .ToListAsync(),
                RecentMaintenances = await _context.Maintenances
                    .Include(m => m.Car)
                    .Include(m => m.Owner)
                    .OrderByDescending(m => m.Date)
                    .Take(5)
                    .ToListAsync()
            };

            return View(stats);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .Include(u => u.Garages)
                .Include(u => u.Cars)
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Garages)
                .Include(u => u.Cars)
                .Include(u => u.Maintenances)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Reports()
        {
            var reports = new AdminReportsViewModel
            {
                UserStatistics = await _userManager.Users
                    .Select(u => new UserStatistics
                    {
                        UserId = u.Id,
                        UserEmail = u.Email,
                        GaragesCount = u.Garages.Count,
                        CarsCount = u.Cars.Count,
                        MaintenancesCount = u.Maintenances.Count,
                        TotalMaintenanceCost = u.Maintenances.Sum(m => m.Cost)
                    })
                    .ToListAsync(),

                MaintenanceStatistics = await _context.Maintenances
                    .GroupBy(m => m.Date.Month)
                    .Select(g => new MaintenanceStatistics
                    {
                        Month = g.Key,
                        Count = g.Count(),
                        TotalCost = g.Sum(m => m.Cost)
                    })
                    .OrderBy(m => m.Month)
                    .ToListAsync()
            };

            return View(reports);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnabled = !user.LockoutEnabled;
            user.LockoutEnd = user.LockoutEnabled ? DateTimeOffset.MaxValue : null;

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> ExportUsersReport()
        {
            var users = await _userManager.Users
                .Include(u => u.Garages)
                .Include(u => u.Cars)
                .Include(u => u.Maintenances)
                .ToListAsync();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Email,Imię,Nazwisko,Liczba Garaży,Liczba Samochodów,Liczba Napraw,Suma Kosztów Napraw");

            foreach (var user in users)
            {
                csv.AppendLine($"{user.Email},{user.FirstName},{user.LastName}," +
                    $"{user.Garages.Count},{user.Cars.Count},{user.Maintenances.Count}," +
                    $"{user.Maintenances.Sum(m => m.Cost):C}");
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "users_report.csv");
        }
    }
} 