using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<IReadOnlyList<Room>> GetByHotelIdAsync(int hotelId, CancellationToken ct = default);
        Task<Room?> GetByIdAsync(int roomId, CancellationToken ct = default);
        Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime checkIn,DateTime checkOut, int guestCount, CancellationToken ct = default);
    }
}
