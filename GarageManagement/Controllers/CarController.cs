using Microsoft.AspNetCore.Mvc;
using GarageManagement.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

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
            var cars = _context.GarageCars
                .Where(gc => gc.GarageId == garageId)
                .Select(gc => gc.Car)
                .ToList();
            return View(cars);
        }

        public IActionResult Details(int garageId, int carId)
        {
            var car = _context.GarageCars
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
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GarageDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            services.AddControllersWithViews();
        }

    }
}
