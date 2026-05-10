using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Application.Extensions;

public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registers all application-layer services.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IBookingService, BookingService>();

        return services;
    }
}
