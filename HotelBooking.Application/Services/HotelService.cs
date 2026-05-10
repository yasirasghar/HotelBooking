using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<HotelDto?> FindByNameAsync(string name, CancellationToken ct = default)
        {
            var hotel = await _hotelRepository.GetByNameAsync(name, ct);

            if (hotel is null)
                return null;

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rooms = hotel.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomType = r.Type.ToString(),
                    Capacity = r.Capacity,
                    HotelId = r.HotelId
                }).ToList()
            };
        }
    }
}
