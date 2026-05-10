using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IReadOnlyList<RoomDto>> GetAvailableRoomsAsync(RoomAvailabilityRequest request, CancellationToken ct = default)
        {
            if (request.CheckIn.Date >= request.CheckOut.Date)
                throw new ArgumentException("Check-out must be after check-in.");

            if (request.CheckIn.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Check-in cannot be in the past.");

            if (request.GuestCount < 1)
                throw new ArgumentException("Guest count must be at least 1.");

            var rooms = await _roomRepository.GetAvailableRoomsAsync(request.CheckIn.Date, request.CheckOut.Date, request.GuestCount, ct);

            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomType = r.Type.ToString(),
                Capacity = r.Capacity,
                HotelId = r.HotelId
            }).ToList();
        }
    }
}
