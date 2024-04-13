using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using EasySettle.Data;
using EasySettle.Utilities;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Encapsulate service configuration
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

ConfigureMiddlewareAndRoutes(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
{
    services.AddControllersWithViews().AddMicrosoftIdentityUI();

    // Move the AddUserSecrets call to the correct place
    if (env.IsDevelopment())
    {
        builder.Configuration.AddUserSecrets<Program>();
    }

    var sqlConnection = configuration.GetConnectionString("Easysettle:SqlDb");
    var storageConnection = configuration.GetConnectionString("Easysettle:Storage");

    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnection));
    services.AddAzureClients(azureBuilder => azureBuilder.AddBlobServiceClient(storageConnection));
    services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAdB2C"));
    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    });

    services.AddRazorPages().AddMicrosoftIdentityUI();
    services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
}

void ConfigureMiddlewareAndRoutes(WebApplication app)
{
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

    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
    app.MapBlazorHub();

    SeedDatabase(app);
}



void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
            // ...rest of the seed method...
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}
