using Microsoft.AspNetCore.Mvc;
using GarageManagement.Models;
using System.Linq;

namespace GarageManagement.Controllers
{
    public class GarageController : Controller
    {
        private readonly GarageDbContext _context;

        public GarageController(GarageDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var garages = _context.Garage.ToList();
            return View(garages);
        }

        public IActionResult Details(int id)
        {
            var garage = _context.Garage.Find(id);
            if (garage == null)
            {
                return NotFound();
            }
            return View(garage);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Garage garage)
        {
            if (ModelState.IsValid)
            {
                _context.Garage.Add(garage);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(garage);
        }

        public IActionResult Edit(int id)
        {
            var garage = _context.Garage.Find(id);
            if (garage == null)
            {
                return NotFound();
            }
            return View(garage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Garage garage)
        {
            if (id != garage.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(garage);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(garage);
        }

        public IActionResult Delete(int id)
        {
            var garage = _context.Garage.Find(id);
            if (garage == null)
            {
                return NotFound();
            }
            return View(garage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var garage = _context.Garage.Find(id);
            if (garage == null)
            {
                return NotFound();
            }

            _context.Garage.Remove(garage);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
