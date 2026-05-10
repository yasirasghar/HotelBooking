using HotelBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IRoomService
    {
        Task<IReadOnlyList<RoomDto>> GetAvailableRoomsAsync(RoomAvailabilityRequest request, CancellationToken ct = default);
    }
}
