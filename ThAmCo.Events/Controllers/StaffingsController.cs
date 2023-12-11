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

        // GET: Staffings
        public async Task<IActionResult> Index()
        {
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

            return View(staffings);
        }

        // GET: Staffings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffing = await _context.Staffings
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            if (staffing == null)
            {
                return NotFound();
            }

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

            return View(staffingViewModel);
        }

        // GET: Staffings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title");
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffingId,StaffId,EventId,AssignmentDate,Role")] Staffing staffing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", staffing.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FirstName", staffing.StaffId);
            return View(staffing);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffing = await _context.Staffings
                .Include(s => s.Staff)
                .Include(s => s.Event)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            if (staffing == null)
            {
                return NotFound();
            }

            var staffingViewModel = new StaffingViewModel
            {
                StaffingId = staffing.StaffingId,
                AssignmentDate = staffing.AssignmentDate,
                Role = staffing.Role
            };

            return View(staffingViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffingId,AssignmentDate,Role")] StaffingViewModel staffingViewModel)
        {
            if (id != staffingViewModel.StaffingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingStaffing = await _context.Staffings.FindAsync(id);

                    if (existingStaffing == null)
                    {
                        return NotFound();
                    }

                    existingStaffing.AssignmentDate = staffingViewModel.AssignmentDate;
                    existingStaffing.Role = staffingViewModel.Role;

                    _context.Update(existingStaffing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffingExists(staffingViewModel.StaffingId))
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

            return View(staffingViewModel);
        }

        // GET: Staffings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Staffings == null)
            {
                return NotFound();
            }

            var staffing = await _context.Staffings
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffingId == id);
            if (staffing == null)
            {
                return NotFound();
            }

            return View(staffing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Staffings == null)
            {
                return Problem("Entity set 'EventsDbContext.Staffings'  is null.");
            }
            var staffing = await _context.Staffings.FindAsync(id);
            if (staffing != null)
            {
                _context.Staffings.Remove(staffing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveStaffFromEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffing = await _context.Staffings
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffingId == id);

            if (staffing == null)
            {
                return NotFound();
            }

            var staffingViewModel = new StaffingViewModel
            {
                StaffingId = staffing.StaffingId,
                AssignmentDate = staffing.AssignmentDate,
                Role = staffing.Role
            };

            return View(staffingViewModel);
        }

        [HttpPost, ActionName("ConfirmRemoveStaffFromEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRemoveStaffFromEvent(int id)
        {
            var staffing = await _context.Staffings.FindAsync(id);

            if (staffing == null)
            {
                return NotFound();
            }

            // Detach the staff member from the event
            staffing.Staff = null;
            staffing.Event = null;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool StaffingExists(int id)
        {
            return _context.Staffings.Any(e => e.StaffingId == id);
        }
    }
}