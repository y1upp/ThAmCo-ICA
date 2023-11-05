
/*
 * Menu class represents a food menu
 */

namespace ThAmCo.Catering.Data
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<MenuFoodItem> MenuFoodItems { get; set; } // Navigation property for MenuFoodItems
    }
}
