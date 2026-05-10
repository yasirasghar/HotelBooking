using HotelBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(BookingRequest request, CancellationToken ct = default);
        Task<BookingResponse?> GetByReferenceAsync(string reference, CancellationToken ct = default);
    }
}
