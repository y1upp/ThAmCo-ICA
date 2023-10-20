﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Venues.Data;

namespace ThAmCo.Venues.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AvailabilityController : ControllerBase
    {
        private readonly VenuesDbContext _context;

        public AvailabilityController(VenuesDbContext context)
        {
            _context = context;
        }

        // GET: api/Availability?eventType=WED&beginDate=2022-11-01&endDate=2022-11-30
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery, MinLength(3), MaxLength(3), Required] string eventType,
                                             [FromQuery, Required] DateTime beginDate,
                                             [FromQuery, Required] DateTime endDate)
        {
            var avails = _context.Availabilities
                                 .Where(a => a.Reservation == null
                                             && a.Date >= beginDate.Date
                                             && a.Date <= endDate.Date)
                                 .Join(_context.Suitabilities.Where(s => s.EventTypeId == eventType),
                                       a => a.VenueCode,
                                       s => s.VenueCode,
                                       (a, s) => new
                                       {
                                           a.Venue.Code,
                                           a.Venue.Name,
                                           a.Venue.Description,
                                           a.Venue.Capacity,
                                           a.Date,
                                           a.CostPerHour
                                       });

            return Ok(await avails.ToListAsync());
        }
    }

}
