
/*
 *  MenuFoodItemDto represents a DTO used for transferring information
 *  about the relationships between a menu and a food item.
 */

namespace ThAmCo.Catering.DTO
{
    public class MenuFoodItemDto
    {
        public int FoodItemId { get; set; }
        public int MenuId { get; set; }
    }
}
