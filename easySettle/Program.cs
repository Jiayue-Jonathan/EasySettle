using easySettle.Data;
using easySettle.Models;
using easySettle.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("appsettings.json");

// add service to container
builder.Services.AddDbContext<ApplicationDbContext>();//right now only for development.json
//if for formal json, default connect has to be set

//other service setting
builder.Services.AddScoped<IGenericRepository<Amenities>, GenericRepository<Amenities>>();
builder.Services.AddScoped<IGenericRepository<BuildingAmenities>, GenericRepository<BuildingAmenities>>();
builder.Services.AddScoped<IGenericRepository<City>, GenericRepository<City>>();
builder.Services.AddScoped<IGenericRepository<Properties>, GenericRepository<Properties>>();
builder.Services.AddScoped<IGenericRepository<PropertyAmenity>, GenericRepository<PropertyAmenity>>();
builder.Services.AddScoped<IGenericRepository<PropertyBuildingAmenity>, GenericRepository<PropertyBuildingAmenity>>();
builder.Services.AddScoped<IGenericRepository<PropertyType>, GenericRepository<PropertyType>>();
//builder.Services.AddScoped<IGenericRepository<Request>, GenericRepository<Request>>();
//this needs to be modify later, for it is google map api related
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

using (var scope = app.Services.CreateScope())
{
    //initial data
    var roleManager =
     scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Collaborator", "Student", "Worker", "Landlord", "Agency" };
    foreach (var role in roles)
    {
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    var userManager =
      scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@demo.com";
    string password = "Abc123@!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();
