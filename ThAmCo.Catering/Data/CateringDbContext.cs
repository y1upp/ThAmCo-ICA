using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
using System.Collections.Generic;
/*
 * CateringDbContext class represents the Entity Framework database
 * conext used to interact with the database. it defines DbSet 
 * properties for each entity and sets up the database relationships and 
 * initial data seeding. 
 */

namespace ThAmCo.Catering.Data
{
    public class CateringDbContext : DbContext
    {
        public DbSet<FoodBooking> FoodBookings { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuFoodItem> MenuFoodItems { get; set; }

        public CateringDbContext(DbContextOptions<CateringDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the relationships between entities and any additional configurations here.
            modelBuilder.Entity<MenuFoodItem>().HasKey(mfi => new { mfi.MenuId, mfi.FoodItemId });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed FoodItems
            modelBuilder.Entity<FoodItem>().HasData(
                new FoodItem { FoodItemId = 1, Description = "Pizza", UnitPrice = 10.99f },
                new FoodItem { FoodItemId = 2, Description = "Burger", UnitPrice = 8.99f }
            // Add more food items as needed
            );
                
            // Seed Menus
            modelBuilder.Entity<Menu>().HasData(
                new Menu { MenuId = 1, MenuName = "Party Menu" },
                new Menu { MenuId = 2, MenuName = "Special Occasion Menu" }
            // Add more menus as needed
            );

            // Seed MenuFoodItems
            modelBuilder.Entity<MenuFoodItem>().HasData(
                new MenuFoodItem { MenuId = 1, FoodItemId = 1 },
                new MenuFoodItem { MenuId = 1, FoodItemId = 2 }
            // Add more menu-food item associations as needed
            );
        }
    }
}