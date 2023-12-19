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

        // GET: Guests/Index
        public IActionResult Index()
        {
            // Retrieve all guests from the database
            var guests = _context.Guests.ToList();

            // Exclude guests that are deleted or recently deleted
            var activeGuests = guests.Where(g => !g.RecentlyDeleted && !g.IsDeleted).ToList();

            // Return the Index view with the list of active guests
            return View(activeGuests);
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the provided id is null or if the Guests DbSet is null
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            // Retrieve a specific guest from the database based on the provided id
            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id);

            // Check if the guest exists
            if (guest == null)
            {
                return NotFound();
            }

            // Return the guest to the Details view
            return View(guest);
        }

        // GET: Guests/Create
        public IActionResult Create()
        {
            // Return the Create view
            return View();
        }

        // POST: Guests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestId,FirstName,LastName,Email,Phone")] Guest guest)
        {
            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                // Add the guest to the database
                _context.Add(guest);
                await _context.SaveChangesAsync();

                // Redirect to the Index action after successful creation
                return RedirectToAction(nameof(Index));
            }

            // If the model is not valid, redisplay the Create form with errors
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the provided id is null or if the Guests DbSet is null
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            // Retrieve the guest from the database based on the provided id
            var guest = await _context.Guests.FindAsync(id);

            // Check if the guest exists
            if (guest == null)
            {
                return NotFound();
            }

            // Return the Edit view with the guest
            return View(guest);
        }

        // POST: Guests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestId,FirstName,LastName,Email,Phone")] Guest guest)
        {
            // Check if the provided id matches the id in the Guest object
            if (id != guest.GuestId)
            {
                return NotFound();
            }

            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the guest in the database
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check for concurrency exception
                    if (!GuestExists(guest.GuestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect to the Index action after successful update
                return RedirectToAction(nameof(Index));
            }

            // If the model is not valid, redisplay the Edit form with errors
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the provided id is null or if the Guests DbSet is null
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            // Retrieve the guest from the database based on the provided id, excluding recently deleted guests
            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id && !m.IsDeleted);

            // Check if the guest exists
            if (guest == null)
            {
                return NotFound();
            }

            // Return the guest to the Delete view
            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the Guests DbSet is null
            if (_context.Guests == null)
            {
                return Problem("Entity set 'EventsDbContext.Guests' is null.");
            }

            // Retrieve the guest from the database based on the provided id
            var guest = await _context.Guests.FindAsync(id);

            // Check if the guest exists
            if (guest != null)
            {
                // Soft delete by setting IsDeleted to true and RecentlyDeleted to true
                guest.IsDeleted = true;
                guest.RecentlyDeleted = true;

                // Update the guest in the database
                _context.Update(guest);
                await _context.SaveChangesAsync();
            }

            // Redirect to the Index action after successful deletion
            return RedirectToAction(nameof(Index));
        }

        // GET: Guests/Anonymize/5
        public IActionResult Anonymize(int id)
        {
            // Retrieve guest information based on id
            var guest = _context.Guests.Find(id);

            // Pass the guest data to the Anonymize view
            return View(guest);
        }

        // POST: Guests/AnonymizeConfirmed/5
        [HttpPost, ActionName("AnonymizeConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnonymizeConfirmed(int id)
        {
            // Retrieve the guest from the database based on the provided id
            var guest = await _context.Guests.FindAsync(id);

            // Check if the guest exists
            if (guest != null)
            {
                // Anonymize the guest and save changes to the database
                guest.Anonymize();
                await _context.SaveChangesAsync();
            }

            // Redirect to the Index action after successful anonymization
            return RedirectToAction(nameof(Index));
        }

        // GET: Guests/ToggleRecentlyDeleted/5
        public IActionResult ToggleRecentlyDeleted(int id)
        {
            // Retrieve the guest from the database based on the provided id
            var guest = _context.Guests.Find(id);

            // Check if the guest exists
            if (guest != null)
            {
                // Toggle the RecentlyDeleted property
                guest.RecentlyDeleted = !guest.RecentlyDeleted;

                // Save changes to the database
                _context.SaveChanges();
            }

            // Redirect to the Index action after toggling RecentlyDeleted
            return RedirectToAction(nameof(Index));
        }

        // GET: Guests/RecentlyDeleted
        public IActionResult RecentlyDeleted()
        {
            // Retrieve a list of recently deleted guests
            var model = _context.Guests.Where(g => g.RecentlyDeleted).ToList();

            // Return the RecentlyDeleted view with the list of recently deleted guests
            return View(model);
        }

        // GET: Guests/Restore/5
        public IActionResult Restore(int id)
        {
            // Retrieve the guest from the database based on the provided id
            var guest = _context.Guests.Find(id);

            // Check if the guest exists
            if (guest != null)
            {
                // Mark as not recently deleted
                guest.RecentlyDeleted = false;

                // Mark as not deleted
                guest.IsDeleted = false;

                // Save changes to the database
                _context.SaveChanges();
            }

            // Redirect to the RecentlyDeleted action after successful restoration
            return RedirectToAction(nameof(RecentlyDeleted));
        }

        // Check if a guest with the provided id exists
        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.GuestId == id);
        }
    }
}