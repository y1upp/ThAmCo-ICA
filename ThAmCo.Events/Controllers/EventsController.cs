using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.ViewModels;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            // Retrieve all events from the database
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the provided id is null or if the Events DbSet is null
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            // Retrieve a specific event from the database based on the provided id
            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);

            // Check if the event exists
            if (@event == null)
            {
                return NotFound();
            }

            // Return the event to the Details view
            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            // Populate ViewBag with available EventTypes for the dropdown
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Title,Date,VenueId,EventTypeId")] Event @event)
        {
            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                // Ensure that the EventType with the specified ID exists
                var eventType = await _context.EventTypes.FindAsync(@event.EventTypeId);
                if (eventType == null)
                {
                    // Add model error if EventType is invalid
                    ModelState.AddModelError("EventTypeId", "Invalid EventType");
                    ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
                    return View(@event);
                }

                // Assign the EventType navigation property
                @event.EventType = eventType;

                // Add the new event to the database
                _context.Add(@event);
                await _context.SaveChangesAsync();

                // Redirect to the Index action after successful creation
                return RedirectToAction(nameof(Index));
            }

            // If the model is not valid, redisplay the form with errors
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the provided id is null or if the Events DbSet is null
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            // Retrieve the event with related EventType from the database based on the provided id
            var @event = await _context.Events.Include(e => e.EventType).FirstOrDefaultAsync(m => m.EventId == id);

            // Check if the event exists
            if (@event == null)
            {
                return NotFound();
            }

            // Populate the ViewBag with the available EventTypes for the dropdown
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventType?.EventTypeId);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,Date,VenueId,EventTypeId")] Event @event)
        {
            // Check if the provided id matches the id in the Event object
            if (id != @event.EventId)
            {
                return NotFound();
            }

            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure that the EventType with the specified ID exists
                    var eventType = await _context.EventTypes.FindAsync(@event.EventTypeId);
                    if (eventType == null)
                    {
                        // Add model error if EventType is invalid
                        ModelState.AddModelError("EventTypeId", "Invalid EventType");
                        ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
                        return View(@event);
                    }

                    // Assign the EventType navigation property
                    @event.EventType = eventType;

                    // Update the event in the database
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check for concurrency exception
                    if (!EventExists(@event.EventId))
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

            // If the model is not valid, redisplay the form with errors
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the provided id is null or if the Events DbSet is null
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            // Retrieve the event from the database based on the provided id
            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);

            // Check if the event exists
            if (@event == null)
            {
                return NotFound();
            }

            // Return the event to the Delete view
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the Events DbSet is null
            if (_context.Events == null)
            {
                return Problem("Entity set 'EventsDbContext.Events' is null.");
            }

            // Retrieve the event from the database based on the provided id
            var @event = await _context.Events.FindAsync(id);

            // Check if the event exists
            if (@event != null)
            {
                // Remove the event from the database
                _context.Events.Remove(@event);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the Index action after successful deletion
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if an event with a specific id exists
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }

        // GET: Events/AddGuest/5
        public IActionResult AddGuest(int id)
        {
            // Retrieve the event based on the provided id
            var @event = _context.Events.Find(id);

            // Check if the event exists
            if (@event == null)
            {
                return NotFound();
            }

            // Retrieve a list of available guests (you may need to adjust this based on your actual model and data structure)
            var availableGuests = _context.Guests.Select(g => new SelectListItem
            {
                Value = g.GuestId.ToString(),
                Text = $"{g.FirstName} {g.LastName}"
            }).ToList();

            // Create the view model with necessary data
            var viewModel = new AddGuestViewModel
            {
                EventId = @event.EventId,
                AvailableGuests = availableGuests
            };

            // Return the view with the view model
            return View(viewModel);
        }

        // POST: Events/AddGuest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGuest(AddGuestViewModel viewModel)
        {
            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                // Retrieve the event based on the provided id
                var @event = await _context.Events.FindAsync(viewModel.EventId);

                // Check if the event exists
                if (@event != null)
                {
                    // Ensure GuestBookings is not null
                    if (@event.GuestBookings == null)
                    {
                        @event.GuestBookings = new List<GuestBooking>();
                    }

                    // Retrieve the selected guest based on the submitted form data
                    var guest = await _context.Guests.FindAsync(viewModel.SelectedGuestId);

                    // Check if the guest exists
                    if (guest != null)
                    {
                        // Add the guest to the event
                        @event.GuestBookings.Add(new GuestBooking
                        {
                            Guest = guest,
                            // Other properties...
                        });

                        // Save changes to the database
                        await _context.SaveChangesAsync();

                        // Redirect to the Details action after successful addition
                        return RedirectToAction(nameof(Details), new { id = viewModel.EventId });
                    }
                }
            }

            // If something went wrong, handle accordingly (e.g., redisplay the form with errors)
            return View(viewModel);
        }

        // GET: Events/AddStaffing/5
        public IActionResult AddStaffing(int id)
        {
            // Retrieve the event based on the provided id
            var @event = _context.Events.Find(id);

            // Check if the event exists
            if (@event == null)
            {
                return NotFound();
            }

            // Retrieve a list of available staff members
            var availableStaffMembers = _context.Staffs
                .Select(s => new SelectListItem
                {
                    Value = s.StaffId.ToString(),
                    Text = $"{s.StaffFirstName} {s.LastName}"
                })
                .ToList();

            // Retrieve a list of available roles (modify this based on your actual roles)
            var availableRoles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Role1", Text = "Role 1" },
                new SelectListItem { Value = "Role2", Text = "Role 2" },
                // Add more roles as needed
            };

            // Create the view model with necessary data
            var viewModel = new AddStaffingViewModel
            {
                EventId = @event.EventId,
                AvailableStaffMembers = availableStaffMembers,
                AvailableRoles = availableRoles
            };

            // Return the view with the view model
            return View(viewModel);
        }

        // POST: Events/AddStaffing/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaffing(AddStaffingViewModel viewModel)
        {
            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                // Retrieve the event based on the provided id
                var @event = await _context.Events.FindAsync(viewModel.EventId);

                // Check if the event exists
                if (@event != null)
                {
                    // Ensure Staffings is not null
                    if (@event.Staffings == null)
                    {
                        @event.Staffings = new List<Staffing>();
                    }

                    // Retrieve the selected staff member based on the submitted form data
                    var staffMember = await _context.Staffs.FindAsync(viewModel.SelectedStaffId);

                    // Check if the staff member exists
                    if (staffMember != null)
                    {
                        // Add the staff member to the event
                        @event.Staffings.Add(new Staffing
                        {
                            Staff = staffMember,
                            Role = viewModel.SelectedRole,
                            AssignmentDate = viewModel.AssignmentDate
                            // Other properties...
                        });

                        // Save changes to the database
                        await _context.SaveChangesAsync();

                        // Redirect to the Details action after successful addition
                        return RedirectToAction(nameof(Details), new { id = viewModel.EventId });
                    }
                }
            }

            // If something went wrong, handle accordingly (e.g., redisplay the form with errors)
            return View(viewModel);
        }
    }
}