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

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public HomeController(AppDbContext context, ILogger<HomeController> logger,BlobServiceClient blobServiceClient)
    {
        _context = context;
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    [Authorize] // Ensure this is only accessible for authenticated users
    public IActionResult Profile()
    {
        // Profile logic here
        // For now, we just return the view
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Index()
    {
        // Passing null to represent fetching properties from all categories
        return await SearchByCategory(null);
    }

public async Task<IActionResult> CombinedSearch(SearchCriteria criteria)
{
    ViewBag.SearchCriteria = criteria;
    IQueryable<Property> query = _context.Properties.Where(p => p.IsAudited); // Only include audited properties

    if (criteria.MinRooms.HasValue || criteria.MaxRooms.HasValue)
    {
        query = query.Where(p => (!criteria.MinRooms.HasValue || p.Rooms >= criteria.MinRooms.Value) && 
                                 (!criteria.MaxRooms.HasValue || p.Rooms <= criteria.MaxRooms.Value));
    }

    if (criteria.MinBathRooms.HasValue || criteria.MaxBathRooms.HasValue)
    {
        query = query.Where(p => (!criteria.MinBathRooms.HasValue || p.BathRooms >= criteria.MinBathRooms.Value) && 
                                 (!criteria.MaxBathRooms.HasValue || p.BathRooms <= criteria.MaxBathRooms.Value));
    }

    if (criteria.MinRent.HasValue || criteria.MaxRent.HasValue)
    {
        query = query.Where(p => (!criteria.MinRent.HasValue || p.Rent >= criteria.MinRent.Value) && 
                                 (!criteria.MaxRent.HasValue || p.Rent <= criteria.MaxRent.Value));
    }

    if (criteria.Type.HasValue)
    {
        query = query.Where(p => p.Type == criteria.Type.Value);
    }

    if (criteria.City.HasValue)
    {
        query = query.Where(p => p.City == criteria.City.Value);
    }

    if (criteria.Parking.HasValue)
    {
        query = query.Where(p => p.Parking == criteria.Parking.Value);
    }

    if (criteria.Pets.HasValue)
    {
        query = query.Where(p => p.Pets == criteria.Pets.Value);
    }

    var properties = await query.ToListAsync();
    var propertyViewModels = await GetPropertyViewModelsAsync(properties);

    return View("SearchResults", propertyViewModels);
}


    public async Task<IActionResult> SearchByCategory(CityEnum? city)
    {
        IEnumerable<Property> properties;

        if (city.HasValue)
        {
            properties = await _context.Properties.Where(c => c.City == city.Value && c.IsAudited).ToListAsync(); // Filter by city and audited
        }
        else
        {
            properties = await _context.Properties.Where(p => p.IsAudited).ToListAsync(); // Only include audited properties
        }

        var propertyViewModels = await GetPropertyViewModelsAsync(properties);

        return View(city.HasValue ? "SearchResults" : "Index", propertyViewModels);
    }


    private async Task<List<PropertyViewModel>> GetPropertyViewModelsAsync(IEnumerable<Property> properties)
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

    public async Task<IActionResult> GetDetails(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var property = await _context.Properties.FirstOrDefaultAsync(m => m.PropertyID == id);
        if (property == null)
        {
            return NotFound(); // Returns a 404 Not Found response if the property doesn't exist
        }

        // Wrap the single property in a collection to use GetPropertyViewModelsAsync
        var propertyCollection = new List<Property> { property };
        var propertyViewModels = await GetPropertyViewModelsAsync(propertyCollection);

        // Since we know there's only one property, we can directly access the first element
        var viewModel = propertyViewModels.FirstOrDefault();

        if (viewModel == null)
        {
            return NotFound(); // Handle the unlikely case that the viewModel is null
        }

        return View("GetDetails", viewModel); // Make sure to use the correct view name
    }


    public IActionResult SomeAction()
    {
        var criteria = new SearchCriteria();
        ViewBag.SearchCriteria = criteria;

        return View();
    }


[HttpPost]
public async Task<IActionResult> ToggleUserProperty(int propertyId, bool isChecked)
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
    _logger.LogWarning("User property list updated successfully for user {UserEmail}.", userEmail); //log is not showed???
    return RedirectToAction("Index");
}



}
