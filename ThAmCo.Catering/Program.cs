using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;

/*
 *  This code configures and sets up a web application that uses 
 *  ASP.NET core to provide web services for the catering project. 
 *  it includes adding services to the container, configuring Entity 
 *  Framework Core for database access, setting up Swagger for API 
 *  documentation, routing and ensuring that the database migrations  
 *  are applied if needed. 
 */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework Core service
builder.Services.AddDbContext<CateringDbContext>(options =>
{
    // Use the default SQLite provider with no specific connection string
    options.UseSqlite("Data Source=mydatabase.db");
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseRouting(); // Add this line for routing

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    // Configure the default route for MVC controllers
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=FoodBookings}/{action=Index}/{id?}");
});

// The following code will ensure that migrations are applied to create the database if it doesn't exist.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<CateringDbContext>();
    dbContext.Database.Migrate(); // Apply migrations and create the database if it doesn't exist.
}

app.Run();