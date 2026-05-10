using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBooking.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly HotelDbContext _context;

    public HotelRepository(HotelDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Case-insensitive name search.
    /// </summary>
    public async Task<Hotel?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        return await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => EF.Functions.Like(h.Name, $"%{name}%"), ct);
    }

    public async Task<Hotel?> GetByIdWithRoomsAsync(int hotelId, CancellationToken ct = default)
    {
        return await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == hotelId, ct);
    }
}
