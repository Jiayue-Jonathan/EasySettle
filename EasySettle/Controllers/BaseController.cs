using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EasySettle.Models;
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
public abstract class BaseController : Controller
{
    protected readonly ILogger<HomeController> _logger;
    protected readonly AppDbContext _context;
    protected readonly BlobServiceClient _blobServiceClient;

    protected BaseController(AppDbContext context, ILogger<HomeController> logger,BlobServiceClient blobServiceClient)
    {
        _context = context;
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    protected async Task<List<PropertyViewModel>> GetPropertyViewModelsAsync(IEnumerable<Property> properties)
    {
        var propertyViewModels = new List<PropertyViewModel>();
                
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

        var userProperties = await _context.UserProperties
                                            .Where(up => up.Email == userEmail)
                                            .Select(up => up.PropertyID)
                                            .ToListAsync();        

        foreach (var property in properties)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(property.PropertyID.ToString());
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobs = containerClient.GetBlobsAsync();
            var blobUrls = new List<string>();

            await foreach (var blobItem in blobs)
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                blobUrls.Add(blobClient.Uri.AbsoluteUri);
            }

            propertyViewModels.Add(new PropertyViewModel
            {
                Property = property,
                ImageUrls = blobUrls,
                // Check if the property ID is in the list of properties for the user
                IsChecked = userProperties.Contains(property.PropertyID)
            });
        }

        return propertyViewModels;
    }
    
    [HttpPost]
    public async Task<IActionResult> ToggleUserProperty(int propertyId, bool isChecked, string redirectToAction = "Index", string redirectToController = "")
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

        _logger.LogInformation("ToggleUserProperty called for user {UserEmail} with property ID {PropertyId} and isChecked set to {IsChecked}.", userEmail, propertyId, isChecked);

        if (string.IsNullOrEmpty(userEmail))
        {
            _logger.LogWarning("Unauthorized access attempt to ToggleUserProperty.");
            return Unauthorized("User is not authenticated.");
        }

        var user = await _context.Users.Include(u => u.UserProperties)
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user == null)
        {
            _logger.LogInformation("Creating new user for email {UserEmail}.", userEmail);
            user = new User
            {
                Email = userEmail,
                UserProperties = new List<UserProperty>()
            };
            _context.Users.Add(user);
        }

        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null)
        {
            _logger.LogWarning("Property with ID {PropertyId} not found.", propertyId);
            return NotFound($"Property with ID {propertyId} not found.");
        }

        var userProperty = user.UserProperties.FirstOrDefault(up => up.PropertyID == propertyId);

        if (isChecked)
        {
            if (userProperty == null)
            {
                _logger.LogInformation("Adding property ID {PropertyId} to user {UserEmail}'s list.", propertyId, userEmail);
                var newUserProperty = new UserProperty { Email = userEmail, PropertyID = propertyId };
                user.UserProperties.Add(newUserProperty);
                _context.UserProperties.Add(newUserProperty);
            }
        }
        else
        {
            if (userProperty != null)
            {
                _logger.LogInformation("Removing property ID {PropertyId} from user {UserEmail}'s list.", propertyId, userEmail);
                user.UserProperties.Remove(userProperty);
                _context.UserProperties.Remove(userProperty);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("User property list updated successfully for user {UserEmail}.", userEmail); // Corrected log level to Information

        // Use the parameters for redirection
        if (!string.IsNullOrEmpty(redirectToController))
        {
            return RedirectToAction(redirectToAction, redirectToController);
        }
        return RedirectToAction(redirectToAction);
    }
    
}
