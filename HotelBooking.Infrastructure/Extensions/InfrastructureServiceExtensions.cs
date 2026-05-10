using HotelBooking.Application.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HotelBooking.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers EF Core, repositories, and the seeder.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HotelDbContext>(options =>
           options.UseSqlite(
               configuration.GetConnectionString("DefaultConnection")
               ?? "Data Source=hotelbooking.db"));

        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<Seeder>();

        return services;
    }
}
