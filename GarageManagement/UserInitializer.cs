using Microsoft.AspNetCore.Identity;
using GarageManagement.Models;
using System.Security.Claims;
using System.Threading.Tasks;

public class UserInitializer
{
    public static async Task InitializeAsync(UserManager<Owner> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("owner"))
        {
            await roleManager.CreateAsync(new IdentityRole("owner"));
        }

        if (!await roleManager.RoleExistsAsync("admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }

        var defaultUser = new Owner
        {
            UserName = "test@example.com",
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

        var adminUser = new Owner
        {
            UserName = "test2@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };

        var admin = await userManager.FindByEmailAsync(adminUser.Email);
        if (admin == null)
        {
            var createAdminResult = await userManager.CreateAsync(adminUser, "AdminPassword123!");
            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "admin");

                await userManager.AddClaimAsync(adminUser, new Claim("ManageUsers", "Allowed"));
                await userManager.AddClaimAsync(adminUser, new Claim("ManageRoles", "Allowed"));
                await userManager.AddClaimAsync(adminUser, new Claim("ViewReports", "Allowed"));
            }
        }
    }
}