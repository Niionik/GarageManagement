using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GarageManagement.Models.ViewModels;

namespace GarageManagement.Controllers
{
    public class CarController : Controller
    {
        private readonly GarageDbContext _context;

        public CarController(GarageDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars
                .Include(c => c.Garage)
                .ToListAsync();
            return View(cars);
        }

        public IActionResult Create()
        {
            ViewData["GarageId"] = new SelectList(_context.Garages, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Year,Mileage,Status,GarageId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GarageId"] = new SelectList(_context.Garages, "Id", "Name", car.GarageId);
            return View(car);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
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

            try
            {
                if (ModelState.IsValid)
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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas aktualizacji danych: " + ex.Message);
            }
            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
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

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMaintenance(MaintenanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = await _context.Cars
                    .Include(c => c.Owner)
                    .FirstOrDefaultAsync(c => c.Id == model.CarId);

                if (car == null)
                {
                    return NotFound();
                }

                // Sprawdź czy użytkownik jest właścicielem samochodu
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (car.OwnerId != userId)
                {
                    return Forbid();
                }

                var maintenance = new Maintenance
                {
                    Date = model.Date,
                    Description = model.Description,
                    Cost = model.Cost,
                    CarId = model.CarId,
                    OwnerId = userId
                };

                _context.Maintenances.Add(maintenance);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Naprawa została dodana do historii.";
                return RedirectToAction(nameof(Details), new { id = model.CarId });
            }

            // Jeśli model jest nieprawidłowy, wróć do widoku szczegółów
            return RedirectToAction(nameof(Details), new { id = model.CarId });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Clone(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Maintenances)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            // Tworzenie kopii samochodu
            var clonedCar = new Car
            {
                Brand = car.Brand,
                Model = $"{car.Model} (Kopia)",
                Year = car.Year,
                Mileage = car.Mileage,
                Status = car.Status,
                WheelModel = car.WheelModel,
                TireSize = car.TireSize,
                TireBrand = car.TireBrand,
                LastOilChange = car.LastOilChange,
                LastTimingBeltChange = car.LastTimingBeltChange,
                OwnerId = car.OwnerId,
                GarageId = car.GarageId
            };

            _context.Cars.Add(clonedCar);
            await _context.SaveChangesAsync();

            // Kopiowanie powiązania z garażem
            if (car.GarageId.HasValue)
            {
                _context.GarageCars.Add(new GarageCar { GarageId = car.GarageId.Value, CarId = clonedCar.Id });
            }

            // Kopiowanie historii napraw
            foreach (var maintenance in car.Maintenances)
            {
                var clonedMaintenance = new Maintenance
                {
                    CarId = clonedCar.Id,
                    Date = maintenance.Date,
                    Description = maintenance.Description,
                    Cost = maintenance.Cost,
                    OwnerId = maintenance.OwnerId
                };
                _context.Maintenances.Add(clonedMaintenance);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Samochód został pomyślnie skopiowany.";
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
