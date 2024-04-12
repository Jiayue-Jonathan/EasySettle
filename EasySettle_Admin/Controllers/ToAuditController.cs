using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasySettle.Data;
using EasySettle.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EasySettle.Controllers;
public class ToAuditController : Controller
{
    private readonly AppDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public ToAuditController(AppDbContext context,BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        
    }
    // GET: ToAudit/Approve/5
    public async Task<IActionResult> Approve(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var property = await _context.Properties.FindAsync(id);
        if (property == null)
        {
            return NotFound();
        }

        if (!property.IsAudited)
        {
            property.IsAudited = true;  // Set IsAudited to true
            property.IsApproved = true; // Assuming you also want to set IsApproved to true

            _context.Update(property);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Property approved successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Property is already audited.";
        }

        return RedirectToAction(nameof(GetToAudit));
    }    
    // GET: Property/CheckPhoto/5
    public async Task<IActionResult> CheckPhoto(int id)
    {
        ViewData["PropertyID"] = id;

        // Get the container client for this property
        var containerClient = _blobServiceClient.GetBlobContainerClient(id.ToString());
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        // Retrieve all blobs in this container
        var blobs = containerClient.GetBlobsAsync();
        var blobUrls = new List<string>();

        // Collect all blob URLs to display them
        await foreach (var blobItem in blobs)
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            blobUrls.Add(blobClient.Uri.AbsoluteUri);
        }

        ViewData["BlobUrls"] = blobUrls; // Pass the list of blob URLs to the view

        return View("CheckPhoto");
    }
    public async Task<IActionResult> Decline(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var property = await _context.Properties.FindAsync(id);
        if (property == null)
        {
            return NotFound();
        }

        if (!property.IsAudited)
        {
            property.IsAudited = true;  // Set IsAudited to true to mark it as processed
            property.IsApproved = false; // Set IsApproved to false to indicate the property has been declined

            _context.Update(property);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Property declined successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Property is already audited.";
        }

        return RedirectToAction(nameof(GetToAudit));
    }
    // GET: ToAudit/GetToAudit
    public async Task<IActionResult> GetToAudit()
    {
        var propertiesToAudit = await _context.Properties
                                    .Where(p => !p.IsAudited)
                                    .ToListAsync();

        return View(propertiesToAudit);
    }
}

