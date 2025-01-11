using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GarageManagement.Models;
using GarageManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GarageManagement.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<Owner> _userManager;
        private readonly SignInManager<Owner> _signInManager;

        public ProfileController(
            UserManager<Owner> userManager,
            SignInManager<Owner> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Sprawdź, czy email nie jest już zajęty przez innego użytkownika
            if (user.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("Email", "Ten adres email jest już zajęty.");
                    return View(model);
                }
            }

            // Aktualizuj dane podstawowe
            var emailChanged = user.Email != model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            if (emailChanged)
            {
                user.UserName = model.Email; // Zaktualizuj też nazwę użytkownika, jeśli email się zmienił
                user.NormalizedUserName = model.Email.ToUpper();
                user.NormalizedEmail = model.Email.ToUpper();
            }

            var result = await _userManager.UpdateAsync(user);

            // Obsługa zmiany hasła
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError(string.Empty, "Aktualne hasło jest wymagane do zmiany hasła.");
                    return View(model);
                }

                // Sprawdź, czy aktualne hasło jest poprawne
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Nieprawidłowe aktualne hasło.");
                    return View(model);
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, 
                    model.CurrentPassword, model.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Twój profil został zaktualizowany.";
                return RedirectToAction(nameof(Edit));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
} 