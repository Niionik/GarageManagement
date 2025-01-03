using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;

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
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Year,Mileage,Status,WheelModel,TireSize,TireBrand")] Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    car.Maintenances = new List<Maintenance>();
                    car.GarageCars = new List<GarageCar>();
                    _context.Add(car);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Samochód został pomyślnie dodany.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas zapisywania danych: " + ex.Message);
            }
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

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
