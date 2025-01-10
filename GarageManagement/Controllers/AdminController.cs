using GarageManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly UserManager<Owner> _userManager;

    public AdminController(UserManager<Owner> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = "Użytkownik został usunięty.";
        }
        else
        {
            TempData["Error"] = "Nie udało się usunąć użytkownika.";
        }
        return RedirectToAction(nameof(Users));
    }
}
