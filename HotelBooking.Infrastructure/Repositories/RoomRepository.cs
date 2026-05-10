using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBooking.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Room>> GetByHotelIdAsync(int hotelId,CancellationToken ct = default)
    {
        return await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync(ct);
    }

    public async Task<Room?> GetByIdAsync(int roomId, CancellationToken ct = default)
    {
        return await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == roomId, ct);
    }

    /// <summary>
    /// Returns rooms that: Have capacity, NO bookings whose date range overlaps
    /// </summary>
    public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut,int guestCount, CancellationToken ct = default)
    {
        var validTypes = Enum.GetValues<RoomType>()
            .Where(t => (int)t >= guestCount)
            .ToList();

        return await _context.Rooms.Include(r => r.Hotel)
                                   .Where(r => validTypes.Contains(r.Type) &&  !r.Bookings
                                   .Any(b =>    b.CheckIn < checkOut && b.CheckOut > checkIn))
                                   .ToListAsync(ct);
    }
}
