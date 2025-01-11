using System.Diagnostics;
using GarageManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var isAuthenticated = User.Identity.IsAuthenticated;

            ViewBag.UserId = userId;
            ViewBag.UserEmail = userEmail;
            ViewBag.IsAuthenticated = isAuthenticated;

            return View();
        }
    }
}
