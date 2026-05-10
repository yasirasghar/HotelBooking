using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByReferenceAsync(string reference, CancellationToken ct = default);
        Task<bool> ReferenceExistsAsync(string reference, CancellationToken ct = default);
        Task<bool> HasOverlappingBookingAsync(int roomId, DateTime checkIn, DateTime checkOut, CancellationToken ct = default);
        Task<Booking> CreateAsync(Booking booking, CancellationToken ct = default);
    }
}
