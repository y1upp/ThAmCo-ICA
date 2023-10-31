using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;

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
            modelBuilder.Entity<MenuFoodItem>()
                .HasKey(mfi => new { mfi.MenuId, mfi.FoodItemId });

            // Seed initial data if needed.
            // modelBuilder.Entity<Menu>().HasData(
            //     new Menu { MenuId = 1, MenuName = "Sample Menu" }
            // );
        }
    }
}