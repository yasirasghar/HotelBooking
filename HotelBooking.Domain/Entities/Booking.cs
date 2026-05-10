using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingReference { get; set; }  // unique GUID
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string GuestName { get; set; }
        public int GuestCount { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
