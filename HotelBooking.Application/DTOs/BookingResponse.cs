using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.DTOs
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public int Nights => (CheckOut - CheckIn).Days;
        public RoomDto Room { get; set; } = null!;
        public string HotelName { get; set; } = string.Empty;
    }
}
