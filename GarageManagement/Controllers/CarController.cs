using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GarageManagement.Controllers
{
    [Authorize(Roles = "owner")]
    public class CarController : Controller
    {
        private readonly GarageDbContext _context;

        public CarController(GarageDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cars = await _context.Cars
                .Where(c => c.OwnerId == userId)
                .Include(c => c.Garage)
                .ToListAsync();
            return View(cars);
        }

        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Year,Mileage,Status,GarageId")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name", car.GarageId);
            return View(car);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null || car.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == car.OwnerId), "Id", "Name", car.GarageId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Year,Mileage,Status,WheelModel,TireSize,TireBrand")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (car.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCar = await _context.Cars.FindAsync(id);
                    if (existingCar == null)
                    {
                        return NotFound();
                    }

                    existingCar.Brand = car.Brand;
                    existingCar.Model = car.Model;
                    existingCar.Year = car.Year;
                    existingCar.Mileage = car.Mileage;
                    existingCar.Status = car.Status;
                    existingCar.WheelModel = car.WheelModel;
                    existingCar.TireSize = car.TireSize;
                    existingCar.TireBrand = car.TireBrand;

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Samochód został pomyślnie zaktualizowany.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Wystąpił błąd podczas aktualizacji danych: " + ex.Message);
                }
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name", car.GarageId);
            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Garage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null || car.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null && car.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Samochód został pomyślnie usunięty.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Maintenances)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null || car.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(car);
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
