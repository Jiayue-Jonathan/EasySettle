using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using EasySettle.Data;
using EasySettle.Models;


using Microsoft.Extensions.Azure;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;



namespace EasySettle.Controllers
{
    public class PropertyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
      
        public PropertyController(AppDbContext context,BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            
        }

    // GET: Property/UploadPhoto/
    public IActionResult UploadPhoto(int id)
    {
        ViewData["PropertyID"] = id;
        
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


            return RedirectToAction(nameof(Index), new { id = id });
        }

        return RedirectToAction("UploadPhoto", new { id = id });
    }






        // GET: Property
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Properties.Include(p => p.Owner);
            return View(await appDbContext.ToListAsync());
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
    public async Task<IActionResult> Create([Bind("PropertyID,OwnerID,Street,City,ZipCode,Type,Rooms,Rent,Rented")] Property property)
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
    public async Task<IActionResult> Edit(int id, [Bind("PropertyID,OwnerID,Street,City,ZipCode,Type,Rooms,Rent,Rented")] Property property)
    {
        if (id != property.PropertyID)
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



        // Other actions...

        private bool PropertyExists(int id)
        {
            return (_context.Properties?.Any(e => e.PropertyID == id)).GetValueOrDefault();
        }
    }
}
