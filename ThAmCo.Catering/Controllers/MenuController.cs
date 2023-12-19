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
public class MenuController : ControllerBase
{
    private readonly CateringDbContext _context;

    // Constructor injection of the database context
    public MenuController(CateringDbContext context)
    {
        _context = context;
    }

    // GET: api/Menu
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuDataDTO>>> GetMenus()
    {
        // Retrieve all menus from the database, including related entities
        var menus = await _context.Menus.Include(m => m.MenuFoodItems).ThenInclude(mfi => mfi.FoodItem).ToListAsync();

        // Map Menu entities to MenuDataDTO objects
        var menuDtos = menus.Select(m => new MenuDataDTO
        {
            MenuId = m.MenuId,
            MenuName = m.MenuName,
            MenuFoodItems = m.MenuFoodItems.Select(mfi => new FoodItemDto
            {
                FoodItemId = mfi.FoodItem.FoodItemId,
                Description = mfi.FoodItem.Description,
                UnitPrice = mfi.FoodItem.UnitPrice
            }).ToList()
        }).ToList();

        // Return the list of MenuDataDTO objects
        return menuDtos;
    }

    // GET: api/Menu/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MenuDataDTO>> GetMenu(int id)
    {
        // Retrieve a specific menu from the database, including related entities, based on the provided id
        var menu = await _context.Menus.Include(m => m.MenuFoodItems).ThenInclude(mfi => mfi.FoodItem).FirstOrDefaultAsync(m => m.MenuId == id);

        // Check if the menu exists
        if (menu == null)
        {
            return NotFound();
        }

        // Map Menu entity to MenuDataDTO object
        var menuDto = new MenuDataDTO
        {
            MenuId = menu.MenuId,
            MenuName = menu.MenuName,
            MenuFoodItems = menu.MenuFoodItems.Select(mfi => new FoodItemDto
            {
                FoodItemId = mfi.FoodItem.FoodItemId,
                Description = mfi.FoodItem.Description,
                UnitPrice = mfi.FoodItem.UnitPrice
            }).ToList()
        };

        // Return the MenuDataDTO object
        return menuDto;
    }

    // POST: api/Menu
    [HttpPost]
    public async Task<ActionResult<MenuDataDTO>> PostMenu(MenuDataDTO menuDto)
    {
        // Create a new Menu entity from the provided MenuDataDTO
        var menu = new Menu
        {
            MenuName = menuDto.MenuName,
            MenuFoodItems = menuDto.MenuFoodItems.Select(mfiDto => new MenuFoodItem
            {
                FoodItem = new FoodItem
                {
                    Description = mfiDto.Description,
                    UnitPrice = mfiDto.UnitPrice
                }
            }).ToList()
        };

        // Add the new Menu entity to the database
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();

        // Update the MenuId in the MenuDataDTO
        menuDto.MenuId = menu.MenuId;

        // Return a 201 Created response with the created MenuDataDTO
        return CreatedAtAction("GetMenu", new { id = menuDto.MenuId }, menuDto);
    }

    // PUT: api/Menu/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMenu(int id, MenuDataDTO menuDto)
    {
        // Check if the provided id matches the id in the MenuDataDTO
        if (id != menuDto.MenuId)
        {
            return BadRequest();
        }

        // Retrieve the menu to be updated from the database, including related entities
        var menu = await _context.Menus
            .Include(m => m.MenuFoodItems)
            .FirstOrDefaultAsync(m => m.MenuId == id);

        // Check if the menu exists
        if (menu == null)
        {
            return NotFound();
        }

        // Update the properties of the Menu entity
        menu.MenuName = menuDto.MenuName;

        // Clear existing MenuFoodItems and add new ones from MenuDataDTO
        menu.MenuFoodItems.Clear();
        menu.MenuFoodItems.AddRange(menuDto.MenuFoodItems.Select(mfiDto => new MenuFoodItem
        {
            FoodItem = new FoodItem
            {
                Description = mfiDto.Description,
                UnitPrice = mfiDto.UnitPrice
            }
        }));

        // Mark the entity as modified and update it in the database
        _context.Entry(menu).State = EntityState.Modified;

        try
        {
            // Save changes to the database
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Check for concurrency exception
            if (!MenuExists(id))
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

    // DELETE: api/Menu/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<MenuDataDTO>> DeleteMenu(int id)
    {
        // Retrieve the menu to be deleted from the database
        var menu = await _context.Menus.FindAsync(id);
       
        // Check if the menu exists
        if (menu == null)
        {
            return NotFound();
        }

        // Remove the menu from the database
        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();

        // Map the deleted Menu entity to MenuDataDTO object
        var deletedMenuDto = new MenuDataDTO
        {
            MenuId = menu.MenuId,
            MenuName = menu.MenuName,
            MenuFoodItems = menu.MenuFoodItems.Select(mfi => new FoodItemDto
            {
                FoodItemId = mfi.FoodItem.FoodItemId,
                Description = mfi.FoodItem.Description,
                UnitPrice = mfi.FoodItem.UnitPrice
            }).ToList()
        };

        // Return the deleted MenuDataDTO object
        return deletedMenuDto;
    }

    // Helper method to check if a menu with a specific id exists
    private bool MenuExists(int id)
    {
        return _context.Menus.Any(e => e.MenuId == id);
    }
}