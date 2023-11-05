using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTO;

/*
 * This controller is responsible for handling HTTP requests related to
 * food items in the catering system. It allows you to perform CRUD operations
 * on food items. It interacts with the database through the CateringDbContext
 * and maps data between data models and Data Transfer Objects
 */


namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public FoodItemController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodItem - gets a list of all food items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
        {
            // Retrieve all food items from the database
            var foodItems = await _context.FoodItems.ToListAsync();

            // Map your data models to DTOs
            var foodItemDtos = foodItems.Select(f => new FoodItemDto
            {
                FoodItemId = f.FoodItemId,
                Description = f.Description,
                UnitPrice = f.UnitPrice
            }).ToList();

            // Return the list of food item DTOs
            return foodItemDtos;
        }

        // GET: api/FoodItem/{id} - Get a specific food item by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetFoodItem(int id)
        {
            // Retrieve a specific food item from the database based on its ID
            var foodItem = await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == id);

            if (foodItem == null) // if item is not found, return a 404 response
            {
                return NotFound();
            }

            // Map the data model to a DTO
            var foodItemDto = new FoodItemDto
            {
                FoodItemId = foodItem.FoodItemId,
                Description = foodItem.Description,
                UnitPrice = foodItem.UnitPrice
            };

            //Return the food item DTO
            return foodItemDto;
        }

        // POST: api/FoodItem - create a new food item
        [HttpPost]
        public async Task<ActionResult<FoodItemDto>> PostFoodItem(FoodItemDto foodItemDto)
        {
            // Map the DTO to a data model
            var foodItem = new FoodItem
            {
                Description = foodItemDto.Description,
                UnitPrice = foodItemDto.UnitPrice
            };

            // add the data model to the database
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();

            // Map the created data model back to a DTO
            var createdFoodItemDto = new FoodItemDto
            {
                FoodItemId = foodItem.FoodItemId,
                Description = foodItem.Description,
                UnitPrice = foodItem.UnitPrice
            };

            // Return a 201 created response with the newly created food item DTO
            return CreatedAtAction("GetFoodItem", new { id = createdFoodItemDto.FoodItemId }, createdFoodItemDto);
        }

        // PUT: api/FoodItem/{id} - Update an existing food item
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodItem(int id, FoodItemDto foodItemDto)
        {
            if (id != foodItemDto.FoodItemId)
            {
                return BadRequest(); // If the ID in the url doesnt match the DTO, return a 400 response
            }

            // Map the DTO to a data model
            var foodItem = new FoodItem
            {
                FoodItemId = foodItemDto.FoodItemId,
                Description = foodItemDto.Description,
                UnitPrice = foodItemDto.UnitPrice
            };

            // Set the entity state to modified and save changes
            _context.Entry(foodItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound(); // If the food item does not exist, return a 404 response
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // return a 204 no content response
        }

        // DELETE: api/FoodItem/{id} - Delete a food item by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<FoodItemDto>> DeleteFoodItem(int id)
        {
            // retreive the food item by ID
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound(); // if the item is not found, return a 404 response
            }

            // remove the food item from the database
            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            // Map the deleted data model to a DTO
            var deletedFoodItemDto = new FoodItemDto
            {
                FoodItemId = foodItem.FoodItemId,
                Description = foodItem.Description,
                UnitPrice = foodItem.UnitPrice
            };

            // return the deleted food item DTO
            return deletedFoodItemDto;
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.FoodItemId == id);
        }
    }
}