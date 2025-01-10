using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GarageManagement.Controllers
{
    [Authorize(Roles = "User")]
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
                garage.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

            var garage = await _context.Garages.FindAsync(id);
            if (garage == null || garage.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
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

            if (garage.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            if (garage == null || garage.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(garage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garage = await _context.Garages.FindAsync(id);
            if (garage != null && garage.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _context.Garages.Remove(garage);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Garaż został pomyślnie usunięty.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GarageExists(int id)
        {
            return _context.Garages.Any(e => e.Id == id);
        }
    }
}