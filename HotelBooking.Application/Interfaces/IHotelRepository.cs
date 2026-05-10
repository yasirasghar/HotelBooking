using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<Hotel?> GetByIdWithRoomsAsync(int hotelId, CancellationToken ct = default);
    }

}
