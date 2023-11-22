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
              return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View();
        }


        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Title,Date,VenueId,EventTypeId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                // Ensure that the EventType with the specified ID exists
                var eventType = await _context.EventTypes.FindAsync(@event.EventTypeId);
                if (eventType == null)
                {
                    ModelState.AddModelError("EventTypeId", "Invalid EventType");
                    ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
                    return View(@event);
                }

                // Assign the EventType navigation property
                @event.EventType = eventType;

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View(@event);
        }


        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.Include(e => e.EventType).FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            // Populate the ViewBag with the available EventTypes for the dropdown
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventType?.EventTypeId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,Date,VenueId,EventTypeId")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure that the EventType with the specified ID exists
                    var eventType = await _context.EventTypes.FindAsync(@event.EventTypeId);
                    if (eventType == null)
                    {
                        ModelState.AddModelError("EventTypeId", "Invalid EventType");
                        ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
                        return View(@event);
                    }

                    // Assign the EventType navigation property
                    @event.EventType = eventType;

                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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

            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'EventsDbContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return _context.Events.Any(e => e.EventId == id);
        }

        // GET: Events/AddGuest/5
        public IActionResult AddGuest(int id)
        {
            var @event = _context.Events.Find(id);

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

            var viewModel = new AddGuestViewModel
            {
                EventId = @event.EventId,
                AvailableGuests = availableGuests
            };

            return View(viewModel);
        }

        // POST: Events/AddGuest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGuest(AddGuestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var @event = await _context.Events.FindAsync(viewModel.EventId);

                if (@event != null)
                {
                    // Ensure GuestBookings is not null
                    if (@event.GuestBookings == null)
                    {
                        @event.GuestBookings = new List<GuestBooking>();
                    }

                    // Retrieve the selected guest based on the submitted form data
                    var guest = await _context.Guests.FindAsync(viewModel.SelectedGuestId);

                    if (guest != null)
                    {
                        // Add the guest to the event
                        @event.GuestBookings.Add(new GuestBooking
                        {
                            Guest = guest,
                            // Other properties...
                        });

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Details), new { id = viewModel.EventId });
                    }
                }
            }

            // If something went wrong, handle accordingly (e.g., redisplay the form with errors)
            return View(viewModel);
        }

        public IActionResult AddStaffing(int id)
        {
            var @event = _context.Events.Find(id);

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

            var viewModel = new AddStaffingViewModel
            {
                EventId = @event.EventId,
                AvailableStaffMembers = availableStaffMembers,
                AvailableRoles = availableRoles
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaffing(AddStaffingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var @event = await _context.Events.FindAsync(viewModel.EventId);

                if (@event != null)
                {
                    // Ensure Staffings is not null
                    if (@event.Staffings == null)
                    {
                        @event.Staffings = new List<Staffing>();
                    }

                    // Retrieve the selected staff member based on the submitted form data
                    var staffMember = await _context.Staffs.FindAsync(viewModel.SelectedStaffId);

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

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Details), new { id = viewModel.EventId });
                    }
                }
            }

            // If something went wrong, handle accordingly (e.g., redisplay the form with errors)
            return View(viewModel);
        }



    }
}
