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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventDetail = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventDetail == null)
            {
                return NotFound();
            }

            return View(eventDetail);
        }

        public IActionResult Index()
        {
            var events = _context.Events.OrderByDescending(e => e.Date).Take(5).ToList();
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}