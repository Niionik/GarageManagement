using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GarageManagement.Controllers
{
    [Authorize(Roles = "User,Administrator")]
    public class CarController : Controller
    {
        private readonly GarageDbContext _context;

        public CarController(GarageDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cars = await _context.Cars
                .Where(c => c.OwnerId == userId)
                .Include(c => c.Garage)
                .ToListAsync();
            return View(cars);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var car = await _context.Cars
                .Include(c => c.Garage)
                .FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Make,Model,Year,GarageId")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name", car.GarageId);
            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);

            if (car == null)
            {
                return NotFound();
            }

            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name", car.GarageId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,GarageId")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    car.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GarageId"] = new SelectList(_context.Garages.Where(g => g.OwnerId == userId), "Id", "Name", car.GarageId);
            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var car = await _context.Cars
                .Include(c => c.Garage)
                .FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);

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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);

            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
