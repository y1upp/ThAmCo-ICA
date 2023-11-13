
/*
 * MenuDto represents a DTO used for transferring information
 * about a menu.
 */

namespace ThAmCo.Catering.DTO
{
    public class MenuDataDTO
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<FoodItemDto> MenuFoodItems { get; set; }
    }
}