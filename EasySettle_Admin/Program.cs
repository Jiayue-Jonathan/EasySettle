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

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

// Add user secrets configuration source
builder.Configuration.AddUserSecrets<Program>();

// Add database context to the container.
var sqlConnection = builder.Configuration.GetConnectionString("Easysettle:SqlDb");
var storageConnection = builder.Configuration.GetConnectionString("Easysettle:Storage");


// GoogleAuth
var clientId = builder.Configuration.GetConnectionString("GoogleAuth:ClientId");
var clientSecret = builder.Configuration.GetConnectionString("GoogleAuth:ClientSecret");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConnection));

builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddBlobServiceClient(storageConnection);
});

// Configure Azure AD B2C authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

// // Additional configuration for saving tokens and configuring OpenID Connect options
// builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
// {
//     options.SaveTokens = true;

//     // Additional OpenID Connect options configuration here
//     // e.g., options.TokenValidationParameters.NameClaimType = "name";
// });



builder.Services.AddAuthorization(options =>
{
    // Create a policy for users
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
    
    // Create a policy for administrators
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});


// Add RazorPages and Blazor services here
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();
builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication middleware should be called before UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapBlazorHub();

// Call the SeedDatabase method here, after building the app and before running it.
SeedDatabase(app);

app.Run();

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
