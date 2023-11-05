
/*
 * FoodItemDto represents a data transfer object used for 
 * transferring infromation about a food item
 */

namespace ThAmCo.Catering.DTO
{
    public class FoodItemDto
    {
        public int FoodItemId { get; set; }
        public string Description { get; set; }
        public float UnitPrice { get; set; }
    }
}