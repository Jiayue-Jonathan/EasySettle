using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasySettle.Data;
using EasySettle.Models;

namespace EasySettle.Controllers
{
    public class LeaseController : Controller
    {
        private readonly AppDbContext _context;

        public LeaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lease
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Leases.Include(l => l.Client).Include(l => l.Property);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Lease/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Client)
                .Include(l => l.Property)
                .FirstOrDefaultAsync(m => m.LeaseID == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Lease/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientID");
            ViewData["PropertyID"] = new SelectList(_context.Properties, "PropertyID", "PropertyID");
            return View();
        }

        // POST: Lease/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaseID,DepositPaid,RentStart,RentFinish,PropertyID,ClientID")] Lease lease)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lease);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientID", lease.ClientID);
            ViewData["PropertyID"] = new SelectList(_context.Properties, "PropertyID", "PropertyID", lease.PropertyID);
            return View(lease);
        }

        // GET: Lease/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientID", lease.ClientID);
            ViewData["PropertyID"] = new SelectList(_context.Properties, "PropertyID", "PropertyID", lease.PropertyID);
            return View(lease);
        }

        // POST: Lease/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaseID,DepositPaid,RentStart,RentFinish,PropertyID,ClientID")] Lease lease)
        {
            if (id != lease.LeaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseExists(lease.LeaseID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientID", lease.ClientID);
            ViewData["PropertyID"] = new SelectList(_context.Properties, "PropertyID", "PropertyID", lease.PropertyID);
            return View(lease);
        }

        // GET: Lease/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Client)
                .Include(l => l.Property)
                .FirstOrDefaultAsync(m => m.LeaseID == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // POST: Lease/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leases == null)
            {
                return Problem("Entity set 'AppDbContext.Leases'  is null.");
            }
            var lease = await _context.Leases.FindAsync(id);
            if (lease != null)
            {
                _context.Leases.Remove(lease);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseExists(int id)
        {
          return (_context.Leases?.Any(e => e.LeaseID == id)).GetValueOrDefault();
        }
    }
}
