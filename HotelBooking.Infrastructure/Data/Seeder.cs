using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBooking.Infrastructure.Data;

/// <summary>
/// Provides seed and reset functionality for testing.
/// Exposed via POST /api/data/seed and DELETE /api/data/reset.
/// </summary>
public class Seeder
{
    private readonly HotelDbContext _context;

    public Seeder(HotelDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Wipes all data and repopulates
    /// </summary>
    public async Task SeedAsync(CancellationToken ct = default)
    {
        await ResetAsync(ct);

        var hotels = new List<Hotel>
        {
            new()
            {
                Name = "The Grand Plaza",
                Address = "1 Grand Plaza, London, EC1A 1BB",
                Rooms = BuildRooms()
            },
            new()
            {
                Name = "Harbour View Hotel",
                Address = "22 Harbour Lane, Bristol, BS1 4RB",
                Rooms = BuildRooms()
            }
        };

        await _context.Hotels.AddRangeAsync(hotels, ct);
        await _context.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Removes all bookings, rooms, and hotels.
    /// </summary>
    public async Task ResetAsync(CancellationToken ct = default)
    {        
        await _context.Bookings.ExecuteDeleteAsync(ct);
        await _context.Rooms.ExecuteDeleteAsync(ct);
        await _context.Hotels.ExecuteDeleteAsync(ct);
    }

    // Private helpers

    /// <summary>
    /// Business rule: every hotel has exactly 6 rooms, 2 Single, 2 Double, 2 Deluxe.
    /// </summary>
    private static List<Room> BuildRooms() =>
    [
        new Room { Type = RoomType.Single },
        new Room { Type = RoomType.Single },
        new Room { Type = RoomType.Double },
        new Room { Type = RoomType.Double },
        new Room { Type = RoomType.Deluxe },
        new Room { Type = RoomType.Deluxe },
    ];
}
