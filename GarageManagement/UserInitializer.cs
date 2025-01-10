using Microsoft.AspNetCore.Identity;
using GarageManagement.Models;
using System.Threading.Tasks;

public class UserInitializer
{
    public static async Task InitializeAsync(UserManager<Owner> userManager, RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Administrator", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@example.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            var newAdmin = new Owner
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };

            var createAdminResult = await userManager.CreateAsync(newAdmin, "Admin123!");
            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Administrator");
            }
            else
            {
                foreach (var error in createAdminResult.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
            }
        }

        var defaultUserEmail = "user@example.com";
        var defaultUser = await userManager.FindByEmailAsync(defaultUserEmail);
        if (defaultUser == null)
        {
            var newUser = new Owner
            {
                UserName = defaultUserEmail,
                Email = defaultUserEmail,
                FirstName = "Default",
                LastName = "User",
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(newUser, "User123!");
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "User");
            }
            else
            {
                foreach (var error in createUserResult.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
            }
        }
    }
}
