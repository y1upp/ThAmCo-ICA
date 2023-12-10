using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTO;

[ApiController]
[Route("api/[controller]")]
public class FoodItemController : ControllerBase
{
    private readonly CateringDbContext _context;

    public FoodItemController(CateringDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
    {
        var foodItems = await _context.FoodItems.ToListAsync();

        var foodItemDtos = foodItems.Select(f => new FoodItemDto
        {
            FoodItemId = f.FoodItemId,
            Description = f.Description,
            UnitPrice = f.UnitPrice
        }).ToList();

        return foodItemDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FoodItemDto>> GetFoodItem(int id)
    {
        var foodItem = await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == id);

        if (foodItem == null)
        {
            return NotFound();
        }

        var foodItemDto = new FoodItemDto
        {
            FoodItemId = foodItem.FoodItemId,
            Description = foodItem.Description,
            UnitPrice = foodItem.UnitPrice
        };

        return foodItemDto;
    }

    [HttpPost]
    public async Task<ActionResult<FoodItemDto>> PostFoodItem(FoodItemDto foodItemDto)
    {
        var foodItem = new FoodItem
        {
            Description = foodItemDto.Description,
            UnitPrice = foodItemDto.UnitPrice
        };

        _context.FoodItems.Add(foodItem);
        await _context.SaveChangesAsync();

        foodItemDto.FoodItemId = foodItem.FoodItemId;

        return CreatedAtAction("GetFoodItem", new { id = foodItemDto.FoodItemId }, foodItemDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFoodItem(int id, FoodItemDto foodItemDto)
    {
        if (id != foodItemDto.FoodItemId)
        {
            return BadRequest();
        }

        var foodItem = new FoodItem
        {
            FoodItemId = foodItemDto.FoodItemId,
            Description = foodItemDto.Description,
            UnitPrice = foodItemDto.UnitPrice
        };

        _context.Entry(foodItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FoodItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<FoodItemDto>> DeleteFoodItem(int id)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);
        if (foodItem == null)
        {
            return NotFound();
        }

        _context.FoodItems.Remove(foodItem);
        await _context.SaveChangesAsync();

        var deletedFoodItemDto = new FoodItemDto
        {
            FoodItemId = foodItem.FoodItemId,
            Description = foodItem.Description,
            UnitPrice = foodItem.UnitPrice
        };

        return deletedFoodItemDto;
    }

    private bool FoodItemExists(int id)
    {
        return _context.FoodItems.Any(e => e.FoodItemId == id);
    }
}