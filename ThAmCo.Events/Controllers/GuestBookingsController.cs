using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class GuestBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: GuestBookings
        public async Task<IActionResult> Index()
        {
            // Retrieve all guest bookings from the database, including related Event and Guest entities
            var eventsDbContext = _context.GuestBookings.Include(g => g.Event).Include(g => g.Guest);
            return View(await eventsDbContext.ToListAsync());
        }

        // GET: GuestBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the provided id is null or if the GuestBookings DbSet is null
            if (id == null || _context.GuestBookings == null)
            {
                return NotFound();
            }

            // Retrieve a specific guest booking from the database based on the provided id, including related Event and Guest entities
            var guestBooking = await _context.GuestBookings
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(m => m.GuestBookingId == id);

            // Check if the guest booking exists
            if (guestBooking == null)
            {
                return NotFound();
            }

            // Return the guest booking to the Details view
            return View(guestBooking);
        }

        // GET: GuestBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the provided id is null or if the GuestBookings DbSet is null
            if (id == null || _context.GuestBookings == null)
            {
                return NotFound();
            }

            // Retrieve the guest booking from the database based on the provided id
            var guestBooking = await _context.GuestBookings.FindAsync(id);

            // Check if the guest booking exists
            if (guestBooking == null)
            {
                return NotFound();
            }

            // Populate ViewData with the available events and guests for the dropdowns
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", guestBooking.EventId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "FirstName", guestBooking.GuestId);

            // Return the Edit view with the guest booking
            return View(guestBooking);
        }

        // POST: GuestBookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestBookingId,GuestId,EventId,BookingDate")] GuestBooking guestBooking)
        {
            // Check if the provided id matches the id in the GuestBooking object
            if (id != guestBooking.GuestBookingId)
            {
                return NotFound();
            }

            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the guest booking in the database
                    _context.Update(guestBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check for concurrency exception
                    if (!GuestBookingExists(guestBooking.GuestBookingId))
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

            // Populate ViewData with the available events and guests for the dropdowns
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", guestBooking.EventId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "FirstName", guestBooking.GuestId);

            // If the model is not valid, redisplay the Edit form with errors
            return View(guestBooking);
        }

        // GET: GuestBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the provided id is null or if the GuestBookings DbSet is null
            if (id == null || _context.GuestBookings == null)
            {
                return NotFound();
            }

            // Retrieve the guest booking from the database based on the provided id, including related Event and Guest entities
            var guestBooking = await _context.GuestBookings
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(m => m.GuestBookingId == id);

            // Check if the guest booking exists
            if (guestBooking == null)
            {
                return NotFound();
            }

            // Return the guest booking to the Delete view
            return View(guestBooking);
        }

        // POST: GuestBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the GuestBookings DbSet is null
            if (_context.GuestBookings == null)
            {
                return Problem("Entity set 'EventsDbContext.GuestBookings' is null.");
            }

            // Retrieve the guest booking from the database based on the provided id
            var guestBooking = await _context.GuestBookings.FindAsync(id);

            // Check if the guest booking exists
            if (guestBooking != null)
            {
                // Remove the guest booking from the database
                _context.GuestBookings.Remove(guestBooking);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the Index action after successful deletion
            return RedirectToAction(nameof(Index));
        }

        // Check if a guest booking with the provided id exists
        private bool GuestBookingExists(int id)
        {
            return _context.GuestBookings.Any(e => e.GuestBookingId == id);
        }
    }
}