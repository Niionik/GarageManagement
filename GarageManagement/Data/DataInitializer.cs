using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
namespace GarageManagement.Data
{
    public static class DataInitializer
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GarageDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Owner>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Upewnij się, że baza danych jest zaktualizowana
                await context.Database.MigrateAsync();

                // Tworzenie ról
                if (!await roleManager.RoleExistsAsync("owner"))
                {
                    await roleManager.CreateAsync(new IdentityRole("owner"));
                }

                if (!await roleManager.RoleExistsAsync("admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                }

                // Tworzenie użytkownika "owner"
                var defaultUser = new Owner
                {
                    UserName = "owner@example.com",
                    Email = "owner@example.com",
                    FirstName = "Default",
                    LastName = "Owner",
                    EmailConfirmed = true
                };
                if (await userManager.FindByEmailAsync(defaultUser.Email) == null)
                {
                    var createUserResult = await userManager.CreateAsync(defaultUser, "Password123!");
                    if (createUserResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser, "owner");
                    }
                }

                // Tworzenie użytkownika "admin"
                var adminUser = new Owner
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                if (await userManager.FindByEmailAsync(adminUser.Email) == null)
                {
                    var createAdminResult = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                    if (createAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "admin");
                    }
                }

                // Dodawanie przykładowych danych
                if (!context.Garages.Any())
                {
                    var owner = await userManager.FindByEmailAsync("owner@example.com");
                    if (owner != null)
                    {
                        var garage = new Garage
                        {
                            Name = "Garaż Domowy",
                            Location = "Staszow",
                            OwnerId = owner.Id
                        };
                        context.Garages.Add(garage);

                        var cars = new List<Car>
                        {
                            new Car { Brand = "Mazda", Model = "RX8", Year = 2004, Mileage = 45000, Status = "Active", WheelModel = "Sport Alloy R19", TireSize = "295/45 R18", TireBrand = "Michelin", LastOilChange = DateTime.Parse("2023-12-15"), LastTimingBeltChange = DateTime.Parse("2023-06-20"), OwnerId = owner.Id, GarageId = garage.Id },
                            new Car { Brand = "Hyundai", Model = "Tiburon", Year = 2006, Mileage = 78000, Status = "Active", WheelModel = "Standard R17", TireSize = "225/50 R17", TireBrand = "Continental", LastOilChange = DateTime.Parse("2023-11-10"), LastTimingBeltChange = DateTime.Parse("2023-04-15"), OwnerId = owner.Id, GarageId = garage.Id },
                            new Car { Brand = "Toyota", Model = "MR3", Year = 1991, Mileage = 5000, Status = "Active", WheelModel = "AMG R18", TireSize = "235/45 R18", TireBrand = "Pirelli", LastOilChange = DateTime.Parse("2024-01-05"), OwnerId = owner.Id, GarageId = garage.Id }
                        };
                        context.Cars.AddRange(cars);

                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
} 