using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GarageDbContext")
    ?? throw new InvalidOperationException("Connection string 'GarageDbContext' not found.");
builder.Services.AddDbContext<GarageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GarageDbContext")));


builder.Services.AddIdentity<Owner, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<GarageDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Owner>>();

    if (!await roleManager.RoleExistsAsync("owner"))
    {
        await roleManager.CreateAsync(new IdentityRole("owner"));
    }

    if (!await roleManager.RoleExistsAsync("Administrator"))
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
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
        var createAdminResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (createAdminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

public class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"Email to: {email}, Subject: {subject}, Message: {htmlMessage}");
        return Task.CompletedTask;
    }
}
