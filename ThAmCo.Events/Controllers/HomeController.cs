using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;  // Add this for LINQ
using ThAmCo.Events.Models;
using ThAmCo.Events.Data;  // Add this for your DbContext
using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Events.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventsDbContext _context;  // Add this for your DbContext

        public HomeController(ILogger<HomeController> logger, EventsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the event details from the database based on the provided id
            var eventDetail = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);

            // Check if the event details exist
            if (eventDetail == null)
            {
                return NotFound();
            }

            // Return the event details to the Details view
            return View(eventDetail);
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            // Retrieve the latest 5 events from the database, ordered by date
            var events = _context.Events.OrderByDescending(e => e.Date).Take(5).ToList();

            // Return the Index view with the list of events
            return View(events);
        }

        // GET: Home/Privacy
        public IActionResult Privacy()
        {
            // Return the Privacy view
            return View();
        }

        // GET: Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Return the Error view with error details
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}