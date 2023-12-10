using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTO;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly CateringDbContext _context;

    public MenuController(CateringDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuDataDTO>>> GetMenus()
    {
        var menus = await _context.Menus.Include(m => m.MenuFoodItems).ThenInclude(mfi => mfi.FoodItem).ToListAsync();

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

        return menuDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuDataDTO>> GetMenu(int id)
    {
        var menu = await _context.Menus.Include(m => m.MenuFoodItems).ThenInclude(mfi => mfi.FoodItem).FirstOrDefaultAsync(m => m.MenuId == id);

        if (menu == null)
        {
            return NotFound();
        }

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

        return menuDto;
    }

    [HttpPost]
    public async Task<ActionResult<MenuDataDTO>> PostMenu(MenuDataDTO menuDto)
    {
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

        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();

        menuDto.MenuId = menu.MenuId;

        return CreatedAtAction("GetMenu", new { id = menuDto.MenuId }, menuDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMenu(int id, MenuDataDTO menuDto)
    {
        if (id != menuDto.MenuId)
        {
            return BadRequest();
        }

        var menu = await _context.Menus
            .Include(m => m.MenuFoodItems)
            .FirstOrDefaultAsync(m => m.MenuId == id);

        if (menu == null)
        {
            return NotFound();
        }

        menu.MenuName = menuDto.MenuName;

        menu.MenuFoodItems.Clear();
        menu.MenuFoodItems.AddRange(menuDto.MenuFoodItems.Select(mfiDto => new MenuFoodItem
        {
            FoodItem = new FoodItem
            {
                Description = mfiDto.Description,
                UnitPrice = mfiDto.UnitPrice
            }
        }));

        _context.Entry(menu).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MenuExists(id))
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
    public async Task<ActionResult<MenuDataDTO>> DeleteMenu(int id)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu == null)
        {
            return NotFound();
        }

        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();

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

        return deletedMenuDto;
    }

    private bool MenuExists(int id)
    {
        return _context.Menus.Any(e => e.MenuId == id);
    }
}