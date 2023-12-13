using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class GuestsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestsController(EventsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var guests = _context.Guests.ToList();

            // Exclude guests that are deleted or recently deleted
            var activeGuests = guests.Where(g => !g.RecentlyDeleted && !g.IsDeleted).ToList();

            return View(activeGuests);
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // GET: Guests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestId,FirstName,LastName,Email,Phone")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestId,FirstName,LastName,Email,Phone")] Guest guest)
        {
            if (id != guest.GuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.GuestId))
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
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id && !m.IsDeleted);

            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Guests == null)
            {
                return Problem("Entity set 'EventsDbContext.Guests' is null.");
            }

            var guest = await _context.Guests.FindAsync(id);

            if (guest != null)
            {
                // Soft delete by setting IsDeleted to true
                guest.IsDeleted = true;

                // Additionally, mark as RecentlyDeleted
                guest.RecentlyDeleted = true;

                _context.Update(guest);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Anonymize(int id)
        {
            // Retrieve guest information based on id
            var guest = _context.Guests.Find(id);

            // Pass the guest data to the Anonymize view
            return View(guest);
        }

        [HttpPost, ActionName("AnonymizeConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnonymizeConfirmed(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest != null)
            {
                guest.Anonymize();
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ToggleRecentlyDeleted(int id)
        {
            var guest = _context.Guests.Find(id);

            if (guest != null)
            {
                guest.RecentlyDeleted = !guest.RecentlyDeleted;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RecentlyDeleted()
        {
            var model = _context.Guests.Where(g => g.RecentlyDeleted).ToList();
            return View(model);
        }

        public IActionResult Restore(int id)
        {
            var guest = _context.Guests.Find(id);

            if (guest != null)
            {
                // Mark as not recently deleted
                guest.RecentlyDeleted = false;

                // Mark as not deleted
                guest.IsDeleted = false;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(RecentlyDeleted));
        }

        private bool GuestExists(int id)
        {
          return _context.Guests.Any(e => e.GuestId == id);
        }
    }
}
