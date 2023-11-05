using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTO;

/*
 * The MenuController is a part of the catering API and provides a 
 * set of HTTP methods to perform CRUD operations for menus and their 
 * food items.
 */

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public MenuController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/Menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetMenus()
        {
            // Retrieve a list of menus including associated menu food item
            var menus = await _context.Menus.Include(m => m.MenuFoodItems).ThenInclude(mfi => mfi.FoodItem).ToListAsync();

            // Map your data models to DTOs
            var menuDtos = menus.Select(m => new MenuDto
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

        //GET: api/Menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDto>> GetMenu(int id)
        {
            // Retrieve a specific menu ID, including associated menu food items                  
            var menu = await _context.Menus.Include(m => m.MenuFoodItems).ThenInclude(mfi => mfi.FoodItem).FirstOrDefaultAsync(m => m.MenuId == id);

            if (menu == null)
            {
                return NotFound();
            }

            // Map the data model to a DTO
            var menuDto = new MenuDto
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                // Map associated food items to FoodItemDto objects
                MenuFoodItems = menu.MenuFoodItems.Select(mfi => new FoodItemDto
                {
                    FoodItemId = mfi.FoodItem.FoodItemId,
                    Description = mfi.FoodItem.Description,
                    UnitPrice = mfi.FoodItem.UnitPrice
                }).ToList()
            };

            return menuDto;
        }

        // POST: api/Menu
        [HttpPost]
        public async Task<ActionResult<MenuDto>> PostMenu(MenuDto menuDto)
        {
            // Map the DTO to a data model
            var menu = new Menu
            {
                MenuName = menuDto.MenuName,
                //Map associated menu food items with new food items
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

            // Map the created data model back to a DTO
            var createdMenuDto = new MenuDto
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                // Map associated food items to foodItemDto objects
                MenuFoodItems = menu.MenuFoodItems.Select(mfi => new FoodItemDto
                {
                    FoodItemId = mfi.FoodItem.FoodItemId,
                    Description = mfi.FoodItem.Description,
                    UnitPrice = mfi.FoodItem.UnitPrice
                }).ToList()
            };

            return CreatedAtAction("GetMenu", new { id = createdMenuDto.MenuId }, createdMenuDto);
        }

        //PUT: api/Menu/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, MenuDto menuDto)
        {
            if (id != menuDto.MenuId)
            {
                return BadRequest();
            }

            // Map the DTO to a data model
            var menu = new Menu
            {
                MenuId = menuDto.MenuId,
                MenuName = menuDto.MenuName,
                // Map associated menu food items with new food items
                MenuFoodItems = menuDto.MenuFoodItems.Select(mfiDto => new MenuFoodItem
                {
                    FoodItem = new FoodItem
                    {
                        Description = mfiDto.Description,
                        UnitPrice = mfiDto.UnitPrice
                    }
                }).ToList()
            };

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

        // DELETE: api/Menu/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<MenuDto>> DeleteMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            // Map the deleted data model to a DTO
            var deletedMenuDto = new MenuDto
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                // Map associated food items to FoodItemDto objects
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
}
