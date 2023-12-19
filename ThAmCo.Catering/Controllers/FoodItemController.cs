using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTO;

// Define the controller with routing attribute
[ApiController]
[Route("api/[controller]")]
public class FoodItemController : ControllerBase
{
    private readonly CateringDbContext _context;

    // Constructor injection of the database context
    public FoodItemController(CateringDbContext context)
    {
        _context = context;
    }

    // GET: api/FoodItem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
    {
        // Retrieve all food items from the database
        var foodItems = await _context.FoodItems.ToListAsync();
        
        // Map FoodItem entities to FoodItemDto objects
        var foodItemDtos = foodItems.Select(f => new FoodItemDto
        {
            FoodItemId = f.FoodItemId,
            Description = f.Description,
            UnitPrice = f.UnitPrice
        }).ToList();

        // Return the list of FoodItemDto objects
        return foodItemDtos;
    }

    // GET: api/FoodItem/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FoodItemDto>> GetFoodItem(int id)
    {
        // Retrieve a specific food item from the database based on the provided id
        var foodItem = await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == id);
        
        // Check if the food item exists
        if (foodItem == null)
        {
            return NotFound();
        }

        // Map FoodItem entity to FoodItemDto object
        var foodItemDto = new FoodItemDto
        {
            FoodItemId = foodItem.FoodItemId,
            Description = foodItem.Description,
            UnitPrice = foodItem.UnitPrice
        };

        // Return the FoodItemDto object
        return foodItemDto;
    }

    // POST: api/FoodItem
    [HttpPost]
    public async Task<ActionResult<FoodItemDto>> PostFoodItem(FoodItemDto foodItemDto)
    {
        // Create a new FoodItem entity from the provided FoodItemDto
        var foodItem = new FoodItem
        {
            Description = foodItemDto.Description,
            UnitPrice = foodItemDto.UnitPrice
        };
        
        // Add the new FoodItem entity to the database
        _context.FoodItems.Add(foodItem);
        await _context.SaveChangesAsync();

        // Update the FoodItemId in the FoodItemDto
        foodItemDto.FoodItemId = foodItem.FoodItemId;

        // Return a 201 Created response with the created FoodItemDto
        return CreatedAtAction("GetFoodItem", new { id = foodItemDto.FoodItemId }, foodItemDto);
    }

    // PUT: api/FoodItem/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFoodItem(int id, FoodItemDto foodItemDto)
    {
        // Check if the provided id matches the id in the FoodItemDto
        if (id != foodItemDto.FoodItemId)
        {
            return BadRequest();
        }
        
        // Create a new FoodItem entity from the provided FoodItemDto
        var foodItem = new FoodItem
        {
            FoodItemId = foodItemDto.FoodItemId,
            Description = foodItemDto.Description,
            UnitPrice = foodItemDto.UnitPrice
        };

        // Mark the entity as modified and update it in the database
        _context.Entry(foodItem).State = EntityState.Modified;

        try
        {
            // Save changes to the database
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {            
            // Check for concurrency exception
            if (!FoodItemExists(id))
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

    // DELETE: api/FoodItem/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<FoodItemDto>> DeleteFoodItem(int id)
    {
        // Retrieve the food item to be deleted from the database
        var foodItem = await _context.FoodItems.FindAsync(id);
        // Check if the food item exists
        if (foodItem == null)
        {
            return NotFound();
        }

        // Remove the food item from the database
        _context.FoodItems.Remove(foodItem);
        await _context.SaveChangesAsync();

        // Map the deleted FoodItem entity to FoodItemDto object
        var deletedFoodItemDto = new FoodItemDto
        {
            FoodItemId = foodItem.FoodItemId,
            Description = foodItem.Description,
            UnitPrice = foodItem.UnitPrice
        };

        // Return the deleted FoodItemDto object
        return deletedFoodItemDto;
    }

    // Helper method to check if a food item with a specific id exists
    private bool FoodItemExists(int id)
    {
        return _context.FoodItems.Any(e => e.FoodItemId == id);
    }
}