using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;

// Define the controller with routing attribute
namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodBookingsController : ControllerBase
    {
        private readonly CateringDbContext _context;

        // Constructor injection of the database context
        public FoodBookingsController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodBooking>>> GetFoodBookings()
        {
            // Retrieve all food bookings from the database
            return await _context.FoodBookings.ToListAsync();
        }

        // GET: api/FoodBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodBooking>> GetFoodBooking(int id)
        {
            // Retrieve a specific food booking from the database based on the provided id
            var foodBooking = await _context.FoodBookings.FindAsync(id);

            // Check if the food booking exists
            if (foodBooking == null)
            {
                return NotFound();
            }

            // Return the food booking
            return foodBooking;
        }

        // PUT: api/FoodBookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodBooking(int id, FoodBooking foodBooking)
        {
            // Check if the provided id matches the id in the FoodBooking object
            if (id != foodBooking.FoodBookingId)
            {
                return BadRequest();
            }

            // Mark the FoodBooking entity as modified and update it in the database
            _context.Entry(foodBooking).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check for concurrency exception
                if (!FoodBookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            // Return a 204 No Content response
            return NoContent();
        }

        // POST: api/FoodBookings
        [HttpPost]
        public async Task<ActionResult<FoodBooking>> PostFoodBooking(FoodBooking foodBooking)
        {
            // Add the new FoodBooking entity to the database
            _context.FoodBookings.Add(foodBooking);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the created FoodBooking object
            return CreatedAtAction("GetFoodBooking", new { id = foodBooking.FoodBookingId }, foodBooking);
        }

        // DELETE: api/FoodBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodBooking(int id)
        {
            // Retrieve the food booking to be deleted from the database
            var foodBooking = await _context.FoodBookings.FindAsync(id);

            // Check if the food booking exists
            if (foodBooking == null)
            {
                return NotFound();
            }

            // Remove the food booking from the database
            _context.FoodBookings.Remove(foodBooking);
            await _context.SaveChangesAsync();

            // Return a 204 No Content response
            return NoContent();
        }

        // Helper method to check if a food booking with a specific id exists
        private bool FoodBookingExists(int id)
        {
            return _context.FoodBookings.Any(e => e.FoodBookingId == id);
        }
    }
}
