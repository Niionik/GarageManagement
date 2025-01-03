using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Data;
using GarageManagement.Models;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GarageManagementContextConnection") ?? throw new InvalidOperationException("Connection string 'GarageManagementContextConnection' not found.");

builder.Services.AddDbContext<GarageManagementContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<GarageManagementContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<GarageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GarageDbContext")));


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


