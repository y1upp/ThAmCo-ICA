using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class StaffController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Staff/Index
        public async Task<IActionResult> Index()
        {
            // Retrieve a list of all staff members from the database
            var staff = await _context.Staff.ToListAsync();

            // Return the Index view with the list of staff members
            return View(staff);
        }

        // GET: Staff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the details of a specific staff member based on the provided id
            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.StaffId == id);

            // Check if the staff member exists
            if (staff == null)
            {
                return NotFound();
            }

            // Return the Details view with the staff member details
            return View(staff);
        }

        // GET: Staff/Create
        public IActionResult Create()
        {
            // Return the Create view for adding a new staff member
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,StaffFirstName,LastName,Email,PhoneNumber")] Staff staff)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Add the new staff member to the database
                _context.Add(staff);
                await _context.SaveChangesAsync();

                // Redirect to the Index view after successful creation
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the Create view with validation errors
            return View(staff);
        }

        // GET: Staff/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the staff member to be edited based on the provided id
            var staff = await _context.Staff.FindAsync(id);

            // Check if the staff member exists
            if (staff == null)
            {
                return NotFound();
            }

            // Return the Edit view with the details of the staff member to be edited
            return View(staff);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,StaffFirstName,LastName,Email,PhoneNumber")] Staff staff)
        {
            // Check if the provided id matches the staff member id
            if (id != staff.StaffId)
            {
                return NotFound();
            }

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the staff member in the database
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if the staff member still exists in the database
                    if (!StaffExists(staff.StaffId))
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
            return View(staff);
        }

        // GET: Staff/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the staff member to be deleted based on the provided id
            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.StaffId == id);

            // Check if the staff member exists
            if (staff == null)
            {
                return NotFound();
            }

            // Return the Delete view with the details of the staff member to be deleted
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the staff member to be deleted based on the provided id
            var staff = await _context.Staff.FindAsync(id);

            // Check if the staff member exists
            if (staff != null)
            {
                // Remove the staff member from the database
                _context.Staff.Remove(staff);
                await _context.SaveChangesAsync();
            }

            // Redirect to the Index view after successful deletion
            return RedirectToAction(nameof(Index));
        }

        // Check if a staff member with the provided id exists
        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.StaffId == id);
        }
    }
}