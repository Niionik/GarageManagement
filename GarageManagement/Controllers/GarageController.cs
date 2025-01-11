using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GarageManagement.Controllers
{
    [Authorize]
    public class GarageController : Controller
    {
        private readonly GarageDbContext _context;

        public GarageController(GarageDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var garages = await _context.Garages
                .Where(g => g.OwnerId == userId)
                .Include(g => g.GarageCars)
                .ThenInclude(gc => gc.Car)
                .ToListAsync();

            return View(garages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Location")] Garage garage)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine($"Creating garage for user ID: {userId}");

                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("User ID is null or empty");
                    return Problem("Użytkownik nie jest zalogowany.");
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {userId} not found in database");
                    return Problem("Użytkownik nie został znaleziony w bazie danych.");
                }

                garage.OwnerId = userId;
                _context.Add(garage);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Garaż został pomyślnie dodany.";
                return RedirectToAction(nameof(Index));
            }
            return View(garage);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await _context.Garages
                .Include(g => g.Owner)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (garage == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (garage.OwnerId != userId)
            {
                return Forbid();
            }

            return View(garage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location")] Garage garage)
        {
            if (id != garage.Id)
            {
                return NotFound();
            }

            var originalGarage = await _context.Garages.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (originalGarage == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (originalGarage.OwnerId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    garage.OwnerId = originalGarage.OwnerId;
                    _context.Update(garage);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Garaż został pomyślnie zaktualizowany.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarageExists(garage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(garage);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await _context.Garages
                .Include(g => g.GarageCars)
                .ThenInclude(gc => gc.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (garage == null)
            {
                return NotFound();
            }

            return View(garage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garage = await _context.Garages.FindAsync(id);
            if (garage == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (garage.OwnerId != userId)
            {
                return Forbid();
            }

            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Garaż został pomyślnie usunięty.";
            return RedirectToAction(nameof(Index));
        }

        private bool GarageExists(int id)
        {
            return _context.Garages.Any(e => e.Id == id);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Clone(int id)
        {
            var garage = await _context.Garages
                .Include(g => g.GarageCars)
                .ThenInclude(gc => gc.Car)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (garage == null)
            {
                return NotFound();
            }

            // Tworzenie kopii garażu
            var clonedGarage = new Garage
            {
                Name = $"{garage.Name} (Kopia)",
                Location = garage.Location,
                OwnerId = garage.OwnerId
            };

            _context.Garages.Add(clonedGarage);
            await _context.SaveChangesAsync();

            // Kopiowanie powiązanych samochodów
            foreach (var garageCar in garage.GarageCars)
            {
                var clonedCar = new Car
                {
                    Brand = garageCar.Car.Brand,
                    Model = garageCar.Car.Model,
                    Year = garageCar.Car.Year,
                    Mileage = garageCar.Car.Mileage,
                    Status = garageCar.Car.Status,
                    WheelModel = garageCar.Car.WheelModel,
                    TireSize = garageCar.Car.TireSize,
                    TireBrand = garageCar.Car.TireBrand,
                    LastOilChange = garageCar.Car.LastOilChange,
                    LastTimingBeltChange = garageCar.Car.LastTimingBeltChange,
                    OwnerId = garageCar.Car.OwnerId,
                    GarageId = clonedGarage.Id
                };

                _context.Cars.Add(clonedCar);
                _context.GarageCars.Add(new GarageCar { GarageId = clonedGarage.Id, Car = clonedCar });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Garaż został pomyślnie skopiowany.";
            return RedirectToAction(nameof(Index));
        }
    }
}
