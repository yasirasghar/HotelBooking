using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly HotelDbContext _context;

    public BookingRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByReferenceAsync(string reference,CancellationToken ct = default)
    {
        return await _context.Bookings.Include(b => b.Room).ThenInclude(r => r.Hotel)
                             .FirstOrDefaultAsync(b => b.BookingReference == reference.ToUpper(),ct);
    }

    public async Task<bool> ReferenceExistsAsync(string reference, CancellationToken ct = default)
    {
        return await _context.Bookings.AnyAsync(b => b.BookingReference == reference, ct);
    }

    /// <summary>
    /// Checks for an overlapping booking on the same room.
    /// </summary>
    public async Task<bool> HasOverlappingBookingAsync(int roomId,DateTime checkIn,DateTime checkOut, CancellationToken ct = default)
    {
        return await _context.Bookings.AnyAsync(b => b.RoomId == roomId && b.CheckIn < checkOut && b.CheckOut > checkIn, ct);
    }

    public async Task<Booking> CreateAsync(Booking booking, CancellationToken ct = default)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(ct);
        
        return await _context.Bookings.Include(b => b.Room).ThenInclude(r => r.Hotel).FirstAsync(b => b.Id == booking.Id, ct);
    }
}
