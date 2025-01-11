using Microsoft.AspNetCore.Identity;
using GarageManagement.Models;
using System.Security.Claims;
using System.Threading.Tasks;

public class UserInitializer
{
    public static async Task InitializeAsync(UserManager<Owner> userManager, RoleManager<IdentityRole> roleManager, GarageDbContext context)
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
            UserName = "owner@example.com",
            Email = "owner@example.com",
            FirstName = "Default",
            LastName = "Owner",
            EmailConfirmed = true
        };

        var existingUser = await userManager.FindByEmailAsync(defaultUser.Email);
        if (existingUser == null)
        {
            var createUserResult = await userManager.CreateAsync(defaultUser, "Password123!");
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultUser, "owner");
                
                var garage = new Garage
                {
                    Name = "Garaż Domowy",
                    Location = "Staszow",
                    OwnerId = defaultUser.Id
                };
                context.Garages.Add(garage);
                await context.SaveChangesAsync();

                var cars = new[]
                {
                    new Car { Brand = "Mazda", Model = "RX8", Year = 2004, Mileage = 45000, Status = "Active", WheelModel = "Sport Alloy R19", TireSize = "295/45 R18", TireBrand = "Michelin", LastOilChange = DateTime.Parse("2023-12-15"), LastTimingBeltChange = DateTime.Parse("2023-06-20"), OwnerId = defaultUser.Id, GarageId = garage.Id },
                    new Car { Brand = "Hyundai", Model = "Tiburon", Year = 2006, Mileage = 78000, Status = "Active", WheelModel = "Standard R17", TireSize = "225/50 R17", TireBrand = "Continental", LastOilChange = DateTime.Parse("2023-11-10"), LastTimingBeltChange = DateTime.Parse("2023-04-15"), OwnerId = defaultUser.Id, GarageId = garage.Id },
                    new Car { Brand = "Toyota", Model = "MR3", Year = 1991, Mileage = 5000, Status = "Active", WheelModel = "AMG R18", TireSize = "235/45 R18", TireBrand = "Pirelli", LastOilChange = DateTime.Parse("2024-01-05"), OwnerId = defaultUser.Id, GarageId = garage.Id }
                };

                context.Cars.AddRange(cars);
                await context.SaveChangesAsync();

                foreach (var car in cars)
                {
                    context.GarageCars.Add(new GarageCar { GarageId = garage.Id, CarId = car.Id });
                }
                await context.SaveChangesAsync();

                var maintenances = new[]
                {
                    new Maintenance { CarId = cars[0].Id, Date = DateTime.Parse("2023-12-15"), Description = "Wymiana oleju i filtrów", Cost = 450.00M, OwnerId = defaultUser.Id },
                    new Maintenance { CarId = cars[1].Id, Date = DateTime.Parse("2023-11-10"), Description = "Przegląd okresowy", Cost = 350.00M, OwnerId = defaultUser.Id },
                    new Maintenance { CarId = cars[2].Id, Date = DateTime.Parse("2024-01-05"), Description = "Wymiana opon na zimowe", Cost = 200.00M, OwnerId = defaultUser.Id }
                };

                context.Maintenances.AddRange(maintenances);
                await context.SaveChangesAsync();
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
            UserName = "admin@example.com",
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

        existingUser = await userManager.FindByEmailAsync("owner@example.com");
        if (existingUser != null)
        {
            // Dodaj przykładowy garaż
            var garage = new Garage
            {
                Name = "Garaż Domowy",
                Location = "Staszow",
                OwnerId = existingUser.Id
            };
            context.Garages.Add(garage);
            await context.SaveChangesAsync();

            // Dodaj przykładowe samochody
            var cars = new[]
            {
                new Car { Brand = "Mazda", Model = "RX8", Year = 2004, Mileage = 45000, Status = "Active", WheelModel = "Sport Alloy R19", TireSize = "295/45 R18", TireBrand = "Michelin", LastOilChange = DateTime.Parse("2023-12-15"), LastTimingBeltChange = DateTime.Parse("2023-06-20"), OwnerId = existingUser.Id, GarageId = garage.Id },
                new Car { Brand = "Hyundai", Model = "Tiburon", Year = 2006, Mileage = 78000, Status = "Active", WheelModel = "Standard R17", TireSize = "225/50 R17", TireBrand = "Continental", LastOilChange = DateTime.Parse("2023-11-10"), LastTimingBeltChange = DateTime.Parse("2023-04-15"), OwnerId = existingUser.Id, GarageId = garage.Id },
                new Car { Brand = "Toyota", Model = "MR3", Year = 1991, Mileage = 5000, Status = "Active", WheelModel = "AMG R18", TireSize = "235/45 R18", TireBrand = "Pirelli", LastOilChange = DateTime.Parse("2024-01-05"), OwnerId = existingUser.Id, GarageId = garage.Id }
            };

            context.Cars.AddRange(cars);
            await context.SaveChangesAsync();

            // Dodaj powiązania GarageCar
            foreach (var car in cars)
            {
                context.GarageCars.Add(new GarageCar { GarageId = garage.Id, CarId = car.Id });
            }
            await context.SaveChangesAsync();

            // Dodaj przykładowe naprawy
            var maintenances = new[]
            {
                new Maintenance { CarId = cars[0].Id, Date = DateTime.Parse("2023-12-15"), Description = "Wymiana oleju i filtrów", Cost = 450.00M, OwnerId = existingUser.Id },
                new Maintenance { CarId = cars[1].Id, Date = DateTime.Parse("2023-11-10"), Description = "Przegląd okresowy", Cost = 350.00M, OwnerId = existingUser.Id },
                new Maintenance { CarId = cars[2].Id, Date = DateTime.Parse("2024-01-05"), Description = "Wymiana opon na zimowe", Cost = 200.00M, OwnerId = existingUser.Id }
            };

            context.Maintenances.AddRange(maintenances);
            await context.SaveChangesAsync();
        }
    }
} 