
/*
 * MenuFoodItem class represents the relationship between
 * a menu and its associated food items.
 */

namespace ThAmCo.Catering.Data
{
    public class MenuFoodItem
    {
        public int MenuId { get; set; }
        public int FoodItemId { get; set; }
        public Menu Menu { get; set; } // Navigation property for Menu
        public FoodItem FoodItem { get; set; } // Navigation property for FoodItem
    }
}
