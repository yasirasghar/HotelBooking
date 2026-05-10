using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.DTOs
{
    public class BookingRequest
    {
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public string GuestName { get; set; } = string.Empty;
    }
}
