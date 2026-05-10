using HotelBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelService
    {
        Task<HotelDto?> FindByNameAsync(string name, CancellationToken ct = default);
    }
}
