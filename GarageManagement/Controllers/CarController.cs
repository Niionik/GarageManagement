using Microsoft.AspNetCore.Mvc;
using GarageManagement.Models;
using System.Linq;

namespace GarageManagement.Controllers
{
    public class CarController : Controller
    {
        private readonly GarageDbContext _context;

        public CarController(GarageDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int garageId)
        {
            var cars = _context.Garage_Car
                .Where(gc => gc.GarageId == garageId)
                .Select(gc => gc.Car)
                .ToList();
            return View(cars);
        }

        public IActionResult Details(int garageId, int carId)
        {
            var car = _context.Garage_Car
                .Where(gc => gc.GarageId == garageId && gc.CarId == carId)
                .Select(gc => gc.Car)
                .FirstOrDefault();
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        public ActionResult Edit(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Car car)
        {
            if (id != car.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        public ActionResult Delete(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
