using HotelBooking.API.Middleware;
using HotelBooking.Application.Extensions;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

// Application layer
builder.Services.AddApplication();

// Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Hotel Room Booking API",
        Version = "v1",
        Description = """
            RESTful API for searching hotels, checking room availability, and managing bookings.

            **Getting started with testing:**
            1. `POST /api/data/seed` — populate the database with test hotels and rooms
            2. `GET /api/hotels?name=grand` — find a hotel
            3. `GET /api/rooms/available?checkIn=2026-06-01&checkOut=2026-06-05&guests=2` — find rooms
            4. `POST /api/bookings` — create a booking
            5. `DELETE /api/data/reset` — clear all data
            """
    });

    // Pull in XML comments from controllers
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Booking API v1");
    c.RoutePrefix = string.Empty;
});

// Global error handling

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
public partial class Program { }
