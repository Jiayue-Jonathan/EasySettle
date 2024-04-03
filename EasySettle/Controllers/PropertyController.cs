using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasySettle.Data;
using EasySettle.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EasySettle.Controllers;

public class PropertyController : Controller
{
    private readonly AppDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public PropertyController(AppDbContext context,BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        
    }

// GET: Property/UploadPhoto/5
public async Task<IActionResult> UploadPhoto(int id)
{
    ViewData["PropertyID"] = id;

    var containerClient = _blobServiceClient.GetBlobContainerClient(id.ToString());
    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

    var blobs = containerClient.GetBlobsAsync();
    var blobUrls = new List<string>();

    await foreach (var blobItem in blobs)
    {
        var blobClient = containerClient.GetBlobClient(blobItem.Name);
        blobUrls.Add(blobClient.Uri.AbsoluteUri);
    }

    ViewData["BlobUrls"] = blobUrls; // Pass the list of blob URLs to the view

    return View();
}


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UploadPhoto(int id, List<IFormFile> photos)
{

//Console.WriteLine($"Property ID: {id}");

var containerClient = _blobServiceClient.GetBlobContainerClient(id.ToString());

if (photos != null && photos.Count > 0)
{
    foreach (var photo in photos)
    {
        if (photo.Length > 0 && photo.Length <= 2097152) // Limit file size to 2MB (2097152 bytes)
        {
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            //string blobName = "11.jpg"; // Set blob name as "1.jpg
            
            await containerClient.UploadBlobAsync(blobName, photo.OpenReadStream());
        }
        else
        {
            // Handle the case where the file size exceeds the limit
            // You can return an error message or take appropriate action
        }
    }


    return RedirectToAction("UploadPhoto", new { id = id });
}

return RedirectToAction("UploadPhoto", new { id = id });
}



// GET: Property
public async Task<IActionResult> Index()
{
    var appDbContext = _context.Properties.Include(p => p.Owner);
    return View(await appDbContext.ToListAsync());
}

 //default route: /Property/MapSearch

public async Task<IActionResult> MapSearch()
{
    return View();
}

// GET: Property/GetAllProperties
[HttpGet]
public async Task<IActionResult> GetAllProperties(string? city = null, int? minRent = null, int? maxRent = null)
{
    IQueryable<Property> query = _context.Properties;

    // city filter
    if (!string.IsNullOrEmpty(city))
    {
        var cityEnum = Enum.Parse<CityEnum>(city);
        query = query.Where(p => p.City == cityEnum);
    }

    // rent filter
    if (minRent.HasValue)
    {
        query = query.Where(p => p.Rent >= minRent.Value);
    }
    if (maxRent.HasValue)
    {
        query = query.Where(p => p.Rent <= maxRent.Value);
    }

    var properties = await query.ToListAsync();
    return Json(properties);
}

    // GET: Property/Details/5
    public async Task<IActionResult> Details(int? id)
{
    if (id == null || _context.Properties == null)
    {
        return NotFound();
    }

    var property = await _context.Properties
        .Include(p => p.Owner)
        .FirstOrDefaultAsync(m => m.PropertyID == id);
    if (property == null)
    {
        return NotFound();
    }

    return View(property);
}

// GET: Property/Create
public IActionResult Create()
{
    ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerID");
    return View();
}

// POST: Property/Create
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(
    [Bind("PropertyID,OwnerID,Street,City,ZipCode,Type,Rooms,BathRooms,Rent,Rented,Parking,Pets")] Property property)

{
    if (ModelState.IsValid)
    {
        _context.Add(property);
        await _context.SaveChangesAsync();
        
        // Create a blob container for the property
        var containerClient = _blobServiceClient.GetBlobContainerClient(property.PropertyID.ToString());
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        
        return RedirectToAction(nameof(Index));
    }
    ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerID", property.OwnerID);
    return View(property);
}


// GET: Property/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null || _context.Properties == null)
    {
        return NotFound();
    }

    var property = await _context.Properties.FindAsync(id);
    if (property == null)
    {
        return NotFound();
    }
    
    ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerID", property.OwnerID);
    
    return View(property);
}

// POST: Property/Edit/5
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("PropertyID,OwnerID,Street,City,ZipCode,Type,Rooms,BathRooms,Rent,Rented,Parking,Pets")] Property property)
{
    if (id != property.PropertyID || !PropertyExists(id))
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(property);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PropertyExists(property.PropertyID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }
    
    ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerID", property.OwnerID);
    
    return View(property);
}


// GET: Property/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null || _context.Properties == null)
    {
        return NotFound();
    }

    var property = await _context.Properties
        .Include(p => p.Owner) // Include the Owner if you need to display owner details in the delete view
        .FirstOrDefaultAsync(m => m.PropertyID == id);

    if (property == null)
    {
        return NotFound();
    }

    return View(property);
}

// POST: Property/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    if (!PropertyExists(id))
    {
        return NotFound();
    }    
    var property = await _context.Properties.FindAsync(id);
    if (property != null)
    {
        // Optional: Delete the blob container associated with the property
        var containerClient = _blobServiceClient.GetBlobContainerClient(property.PropertyID.ToString());
        await containerClient.DeleteIfExistsAsync();

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeletePhoto(int id, string blobUrl)
{
    if (String.IsNullOrWhiteSpace(blobUrl))
    {
        return BadRequest("Invalid blob URL.");
    }

    // Parse the URL to get the blob name
    Uri uri = new Uri(blobUrl);
    string blobName = Path.GetFileName(uri.LocalPath);

    // Retrieve the container client
    var containerClient = _blobServiceClient.GetBlobContainerClient(id.ToString());
    // Get the blob client
    var blobClient = containerClient.GetBlobClient(blobName);
    // Delete the blob
    await blobClient.DeleteIfExistsAsync();

    return RedirectToAction("UploadPhoto", new { id = id });
}


private bool PropertyExists(int id)
{
    return (_context.Properties?.Any(e => e.PropertyID == id)).GetValueOrDefault();
}

}

