using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.ViewModels;

namespace ThAmCo.Events.Controllers
{
    public class StaffingsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Staffings/Index
        public async Task<IActionResult> Index()
        {
            // Retrieve staffings from the database, including related staff and event information
            var staffings = await _context.Staffings
                .Include(s => s.Staff)
                .Include(s => s.Event)
                .Select(s => new StaffingViewModel
                {
                    StaffingId = s.StaffingId,
                    StaffFirstName = s.Staff.StaffFirstName, // Adjust property names accordingly
                    LastName = s.Staff.LastName,
                    Email = s.Staff.Email,
                    PhoneNumber = s.Staff.PhoneNumber,
                    EventTitle = s.Event.Title,
                    AssignmentDate = s.AssignmentDate,
                    Role = s.Role
                })
                .ToListAsync();

            // Return the Index view with the list of staffings
            return View(staffings);
        }

        // GET: Staffings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve staffing details, including related staff and event information
            var staffing = await _context.Staffings
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            // Check if the staffing exists
            if (staffing == null)
            {
                return NotFound();
            }

            // Create a view model for staffing details
            var staffingViewModel = new StaffingViewModel
            {
                StaffingId = staffing.StaffingId,
                StaffFirstName = staffing.Staff.StaffFirstName, // Adjust property names accordingly
                LastName = staffing.Staff.LastName,
                Email = staffing.Staff.Email,
                PhoneNumber = staffing.Staff.PhoneNumber,
                EventTitle = staffing.Event.Title,
                AssignmentDate = staffing.AssignmentDate,
                Role = staffing.Role
            };

            // Return the Details view with the staffing details
            return View(staffingViewModel);
        }

        // GET: Staffings/Create
        public IActionResult Create()
        {
            // Provide dropdown lists for selecting event and staff members
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title");
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FirstName");

            // Return the Create view for adding a new staffing
            return View();
        }

        // POST: Staffings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffingId,StaffId,EventId,AssignmentDate,Role")] Staffing staffing)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Add the new staffing to the database
                _context.Add(staffing);
                await _context.SaveChangesAsync();

                // Redirect to the Index view after successful creation
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the Create view with validation errors
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", staffing.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FirstName", staffing.StaffId);
            return View(staffing);
        }

        // GET: Staffings/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the staffing to be edited, including related staff and event information
            var staffing = await _context.Staffings
                .Include(s => s.Staff)
                .Include(s => s.Event)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            // Check if the staffing exists
            if (staffing == null)
            {
                return NotFound();
            }

            // Create a view model for editing staffing details
            var staffingViewModel = new StaffingViewModel
            {
                StaffingId = staffing.StaffingId,
                AssignmentDate = staffing.AssignmentDate,
                Role = staffing.Role
            };

            // Return the Edit view with the details of the staffing to be edited
            return View(staffingViewModel);
        }

        // POST: Staffings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffingId,AssignmentDate,Role")] StaffingViewModel staffingViewModel)
        {
            // Check if the provided id matches the staffing id
            if (id != staffingViewModel.StaffingId)
            {
                return NotFound();
            }

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing staffing from the database
                    var existingStaffing = await _context.Staffings.FindAsync(id);

                    // Check if the staffing still exists in the database
                    if (existingStaffing == null)
                    {
                        return NotFound();
                    }

                    // Update the staffing details
                    existingStaffing.AssignmentDate = staffingViewModel.AssignmentDate;
                    existingStaffing.Role = staffingViewModel.Role;

                    // Update the staffing in the database
                    _context.Update(existingStaffing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if the staffing still exists in the database
                    if (!StaffingExists(staffingViewModel.StaffingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect to the Index view after successful update
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the Edit view with validation errors
            return View(staffingViewModel);
        }

        // GET: Staffings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the provided id is null or if the Staffings entity set is null
            if (id == null || _context.Staffings == null)
            {
                return NotFound();
            }

            // Retrieve the staffing to be deleted, including related staff and event information
            var staffing = await _context.Staffings
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            // Check if the staffing exists
            if (staffing == null)
            {
                return NotFound();
            }

            // Return the Delete view with the details of the staffing to be deleted
            return View(staffing);
        }

        // POST: Staffings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the Staffings entity set is null
            if (_context.Staffings == null)
            {
                return Problem("Entity set 'EventsDbContext.Staffings' is null.");
            }

            // Retrieve the staffing to be deleted
            var staffing = await _context.Staffings.FindAsync(id);

            // Check if the staffing exists
            if (staffing != null)
            {
                // Remove the staffing from the database
                _context.Staffings.Remove(staffing);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the Index view after successful deletion
            return RedirectToAction(nameof(Index));
        }

        // GET: Staffings/RemoveStaffFromEvent/5
        [HttpGet]
        public async Task<IActionResult> RemoveStaffFromEvent(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the staffing to remove from an event, including related staff and event information
            var staffing = await _context.Staffings
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            // Check if the staffing exists
            if (staffing == null)
            {
                return NotFound();
            }

            // Create a view model for removing staff from an event
            var staffingViewModel = new StaffingViewModel
            {
                StaffingId = staffing.StaffingId,
                AssignmentDate = staffing.AssignmentDate,
                Role = staffing.Role
            };

            // Return the view for removing staff from an event with the staffing details
            return View(staffingViewModel);
        }

        // POST: Staffings/ConfirmRemoveStaffFromEvent/5
        [HttpPost, ActionName("ConfirmRemoveStaffFromEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRemoveStaffFromEvent(int id)
        {
            // Retrieve the staffing to remove from an event
            var staffing = await _context.Staffings.FindAsync(id);

            // Check if the staffing exists
            if (staffing == null)
            {
                return NotFound();
            }

            // Detach the staff member from the event
            staffing.Staff = null;
            staffing.Event = null;

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the Index view after successfully removing staff from an event
            return RedirectToAction(nameof(Index));
        }

        // Check if a staffing with the provided id exists
        private bool StaffingExists(int id)
        {
            return _context.Staffings.Any(e => e.StaffingId == id);
        }
    }
}