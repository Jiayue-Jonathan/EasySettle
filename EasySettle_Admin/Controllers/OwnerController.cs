using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasySettle.Data;
using EasySettle.Models;

namespace EasySettle.Controllers;

public class OwnerController : Controller
{
    private readonly AppDbContext _context;

    public OwnerController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Owner
    public async Task<IActionResult> Index()
    {
            return _context.Owners != null ? 
                        View(await _context.Owners.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Owners'  is null.");
    }

    // GET: Owner/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Owners == null)
        {
            return NotFound();
        }

        var owner = await _context.Owners
            .FirstOrDefaultAsync(m => m.OwnerID == id);
        if (owner == null)
        {
            return NotFound();
        }

        return View(owner);
    }

    // GET: Owner/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Owner/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OwnerID,Street,City,ZipCode,telNo")] Owner owner)
{
    try
    {
        if (ModelState.IsValid)
        {
            _context.Add(owner);
            await _context.SaveChangesAsync();
            Console.WriteLine("Owner created successfully.");
            return RedirectToAction(nameof(Index));
        }
        else
        {
            // Log validation errors
            foreach (var key in ModelState.Keys)
            {
                var entry = ModelState[key];
                if (entry != null && entry.Errors != null) // Nullable check added here
                {
                    foreach (var error in entry.Errors)
                    {
                        Console.WriteLine($"Validation error for {key}: {error.ErrorMessage}");
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while creating the owner: {ex.Message}");
    }
    // If execution reaches here, there was a problem, return to the create view
    return View(owner);
}



    // GET: Owner/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Owners == null)
        {
            return NotFound();
        }

        var owner = await _context.Owners.FindAsync(id);
        if (owner == null)
        {
            return NotFound();
        }
        return View(owner);
    }

    // POST: Owner/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("OwnerID,Street,City,ZipCode,telNo")] Owner owner)
    {
        if (id != owner.OwnerID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(owner);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(owner.OwnerID))
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
        return View(owner);
    }

    // GET: Owner/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Owners == null)
        {
            return NotFound();
        }

        var owner = await _context.Owners
            .FirstOrDefaultAsync(m => m.OwnerID == id);
        if (owner == null)
        {
            return NotFound();
        }

        return View(owner);
    }

    // POST: Owner/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Owners == null)
        {
            return Problem("Entity set 'AppDbContext.Owners'  is null.");
        }
        var owner = await _context.Owners.FindAsync(id);
        if (owner != null)
        {
            _context.Owners.Remove(owner);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OwnerExists(int id)
        {
          return (_context.Owners?.Any(e => e.OwnerID == id)).GetValueOrDefault();
        }
}

