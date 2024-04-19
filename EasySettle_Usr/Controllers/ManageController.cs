using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EasySettle.Models;
using EasySettle.Models.ViewModels;
using EasySettle.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models; // This is for PublicAccessType
using Microsoft.EntityFrameworkCore; // This is for ToListAsync()
using Microsoft.Extensions.Azure;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasySettle.Controllers;

public class ManageController : BaseController
{
    public ManageController(AppDbContext context, ILogger<HomeController> logger, BlobServiceClient blobServiceClient)
        : base(context, logger, blobServiceClient)
        {       
        }
    
public async Task<IActionResult> GetYourProperties()
{
    var userEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
    if (string.IsNullOrEmpty(userEmail))
    {
        return Unauthorized("User is not authenticated.");
    }

    var owner = await _context.Owners
                              .Include(o => o.Properties)
                              .FirstOrDefaultAsync(o => o.Email == userEmail);

    if (owner == null)
    {
        int maxOwnerId = _context.Owners.Any() ? _context.Owners.Max(o => o.OwnerID) + 1 : 20000; // Start IDs from 10001 if none exist

        owner = new Owner 
        { 
            Email = userEmail,
            OwnerID = maxOwnerId  
        };
        _context.Owners.Add(owner);
        await _context.SaveChangesAsync(); 
    }

    if (owner.Properties == null || !owner.Properties.Any())
    {
        TempData["Message"] = "You do not have any properties listed. Consider adding new properties.";
    }

    return View(owner.Properties.ToList());
}


    // GET: Property/Create
    public IActionResult PostYourProperty()
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("Invalid user data.");
        }

        if (!UserIsLandlord())
        {
            return Unauthorized("Access is restricted to landlords only.");
        }

        var viewModel = new PropertyAndOwnerViewModel();

        var owner = _context.Owners.FirstOrDefault(o => o.Email == userEmail);
        if (owner != null)
        {
            viewModel.Owner = owner;
        }
        else
        {
            int maxOwnerId = _context.Owners.Any() ? _context.Owners.Max(o => o.OwnerID) : 20000; // start IDs from 10000 if none exist
            viewModel.Owner = new Owner { Email = userEmail, OwnerID = maxOwnerId + 1 };
        }
        int maxPropertyId = _context.Properties.Any() ? _context.Properties.Max(p => p.PropertyID) : 10000; // Default to 9999 if no properties exist
        viewModel.Property = new Property { PropertyID = maxPropertyId + 1 };
        return View(viewModel);
    }


   // POST: Property/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostYourProperty(PropertyAndOwnerViewModel viewModel)
    {
        if (!UserIsLandlord())
        {
            return Unauthorized("Access is restricted to landlords only.");
        }

        if (ModelState.IsValid)
        {
            // Ensure the Owner is added and saved first if it's new
            if (!_context.Owners.Any(o => o.OwnerID == viewModel.Owner.OwnerID))
            {
                _context.Owners.Add(viewModel.Owner);
                await _context.SaveChangesAsync(); // This ensures OwnerID is generated
            }  else
            {
                _context.Update(viewModel.Owner); // Update existing owner
                await _context.SaveChangesAsync();
            }

            viewModel.Property.OwnerID = viewModel.Owner.OwnerID; // Ensure the correct OwnerID is assigned
            _context.Properties.Add(viewModel.Property);
            await _context.SaveChangesAsync();

            return RedirectToAction("UploadPhoto", "Property", new { id = viewModel.Property.PropertyID });
        }

        return View(viewModel);
    } 
    public IActionResult Profile()
    {
        var model = new UserProfileViewModel
        {
            Email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value,
            DisplayName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
            ObjectId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
            City = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value, 
            Roles = User.Claims.FirstOrDefault(c => c.Type == "extension_Roles")?.Value,
            GivenName = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value,

        };

        return View("Profile",model);
    }

    [Authorize]
    public async Task<IActionResult> YourList()
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("You must be logged in to view your properties.");
        }

        var userProperties = await _context.UserProperties
            .Where(up => up.Email == userEmail)
            .Select(up => up.PropertyID)
            .ToListAsync();

        var properties = await _context.Properties
            .Where(p => userProperties.Contains(p.PropertyID) && p.IsAudited)
            .ToListAsync();

        var propertyViewModels = await GetPropertyViewModelsAsync(properties);

        return View(propertyViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> HandlePropertyToggle(int propertyId, bool isChecked)
    {
        // Define your redirection targets
        var redirectToAction = "YourList";
        var redirectToController = "Manage";

        // Correctly await the call to the asynchronous method
        return await ToggleUserProperty(propertyId, isChecked, redirectToAction, redirectToController);
    }    
    private bool UserIsLandlord()
    {
        var roles = User.Claims.Where(c => c.Type == "extension_Roles")
                            .Select(c => c.Value).ToList();
        
        var rolesString = string.Join(", ", roles);
        var isLandlord = rolesString.Contains("Landlord");

        if (!isLandlord)
        {
            // Optionally, log the attempt or handle other business logic
            _logger.LogInformation($"Access denied. User roles: {rolesString}");
        }

        return isLandlord;
    }


}


