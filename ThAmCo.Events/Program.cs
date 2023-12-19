using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

// Create a new WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the DbContext for the EventsDbContext with SQLite as the database provider
builder.Services.AddDbContext<EventsDbContext>(options =>
{
    options.UseSqlite("Data Source=events.db"); // SQLite database connection string
});

var app = builder.Build(); // Build the WebApplication

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Handle exceptions by redirecting to the Error action in HomeController

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Enable static file serving

app.UseRouting(); // Enable routing

app.UseAuthorization(); // Enable authorization

// Map the default controller route with optional id parameter
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Run the application