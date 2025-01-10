using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using System.Security.Claims;

namespace GarageManagement.Controllers
{
    [Authorize(Roles = "User,Administrator")]
    public class GarageController : Controller
    {
        private readonly GarageDbContext _context;

        public GarageController(GarageDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var garages = await _context.Garages
                .Where(g => g.OwnerId == userId)
                .Include(g => g.GarageCars)
                .ToListAsync();
            return View(garages);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var garage = await _context.Garages
                .Include(g => g.GarageCars)
                .FirstOrDefaultAsync(g => g.Id == id && g.OwnerId == userId);

            if (garage == null)
            {
                return NotFound();
            }

            return View(garage);
        }

        [HttpGet]
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
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User is not logged in");
                garage.OwnerId = userId;
                garage.OwnerId = userId;
                _context.Add(garage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(garage);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var garage = await _context.Garages.FirstOrDefaultAsync(g => g.Id == id && g.OwnerId == userId);

            if (garage == null)
            {
                return NotFound();
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

            if (ModelState.IsValid)
            {
                try
                {
                    garage.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _context.Update(garage);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(garage);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var garage = await _context.Garages
                .Include(g => g.GarageCars)
                .FirstOrDefaultAsync(g => g.Id == id && g.OwnerId == userId);

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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var garage = await _context.Garages.FirstOrDefaultAsync(g => g.Id == id && g.OwnerId == userId);

            if (garage == null)
            {
                return NotFound();
            }

            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GarageExists(int id)
        {
            return _context.Garages.Any(e => e.Id == id);
        }
    }
}
