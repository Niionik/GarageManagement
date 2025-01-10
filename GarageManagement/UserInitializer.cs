using Microsoft.AspNetCore.Identity;
using GarageManagement.Models;
using System.Threading.Tasks;

public class UserInitializer
{
    public static async Task InitializeAsync(UserManager<Owner> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("owner"))
        {
            await roleManager.CreateAsync(new IdentityRole("owner"));
        }

        var defaultUser = new Owner
        {
            UserName = "owner@example.com",
            Email = "owner@example.com",
            FirstName = "Default",
            LastName = "Owner",
            EmailConfirmed = true
        };

        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
            var createUserResult = await userManager.CreateAsync(defaultUser, "Password123!");
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultUser, "owner");
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