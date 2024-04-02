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
    // Make the criteria available to the view for re-rendering the form
    ViewBag.SearchCriteria = criteria;

    // Start with all properties
    IQueryable<Property> query = _context.Properties;

    // Check if the Rooms filter should be applied
    if (criteria.IsRoomsSearchActive)
    {
        query = query.Where(p => (!criteria.MinRooms.HasValue || p.Rooms >= criteria.MinRooms.Value) && 
                                 (!criteria.MaxRooms.HasValue || p.Rooms <= criteria.MaxRooms.Value));
    }

    // Check if the BathRooms filter should be applied
    if (criteria.IsBathRoomsSearchActive)
    {
        query = query.Where(p => (!criteria.MinBathRooms.HasValue || p.BathRooms >= criteria.MinBathRooms.Value) && 
                                 (!criteria.MaxBathRooms.HasValue || p.BathRooms <= criteria.MaxBathRooms.Value));
    }

    // Check if the Rent filter should be applied
    if (criteria.IsRentSearchActive)
    {
        query = query.Where(p => (!criteria.MinRent.HasValue || p.Rent >= criteria.MinRent.Value) && 
                                 (!criteria.MaxRent.HasValue || p.Rent <= criteria.MaxRent.Value));
    }

    // Additional filters can be checked and applied in a similar manner

    // Finalize the query and execute it
    var properties = await query.ToListAsync();
    var propertyViewModels = await GetPropertyViewModelsAsync(properties);

    // Return the view with filtered results
    return View("SearchResults", propertyViewModels);
}


    public async Task<IActionResult> SearchByCategory(CityEnum? city)
    {
        IEnumerable<Property> properties;

        if (city.HasValue)
        {
            properties = await _context.Properties.Where(c => c.City == city.Value).ToListAsync();
        }
        else
        {
            properties = await _context.Properties.ToListAsync();
        }

        var propertyViewModels = await GetPropertyViewModelsAsync(properties);

        // Use a different view when fetching all categories, if needed
        return View(city.HasValue ? "SearchResults" : "Index", propertyViewModels);
    }


    // // For searching by room range
    // public async Task<IActionResult> SearchByRooms(decimal? minRooms, decimal? maxRooms)
    // {
    //     Expression<Func<Property, bool>> condition = null;

    //     if (minRooms.HasValue && maxRooms.HasValue)
    //     {
    //         condition = p => p.Rooms >= minRooms && p.Rooms <= maxRooms;
    //     }
    //     else if (minRooms.HasValue)
    //     {
    //         condition = p => p.Rooms >= minRooms;
    //     }
    //     else if (maxRooms.HasValue)
    //     {
    //         condition = p => p.Rooms <= maxRooms;
    //     }

    //     return await GeneralSearch(condition);
    // }



    // // For searching by rent range
    // public async Task<IActionResult> SearchByRent(decimal? minRent, decimal? maxRent)
    // {
    //     Expression<Func<Property, bool>> condition = null;

    //     if (minRent.HasValue && maxRent.HasValue)
    //     {
    //         condition = p => p.Rent >= minRent && p.Rent <= maxRent;
    //     }
    //     else if (minRent.HasValue)
    //     {
    //         condition = p => p.Rent >= minRent;
    //     }
    //     else if (maxRent.HasValue)
    //     {
    //         condition = p => p.Rent <= maxRent;
    //     }

    //     return await GeneralSearch(condition);
    // }


    // public async Task<IActionResult> GeneralSearch(
    //     Expression<Func<Property, bool>> condition = null)
    // {
    //     IQueryable<Property> query = _context.Properties;

    //     if (condition != null)
    //     {
    //         query = query.Where(condition);
    //     }

    //     var properties = await query.ToListAsync();
    //     var propertyViewModels = await GetPropertyViewModelsAsync(properties);

    //     return View("SearchResults", propertyViewModels);
    // }


    private async Task<List<PropertyViewModel>> GetPropertyViewModelsAsync(IEnumerable<Property> properties)
    {
        var propertyViewModels = new List<PropertyViewModel>();

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
                ImageUrls = blobUrls
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
}
