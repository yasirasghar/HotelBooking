using HotelBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }
        public int Capacity => (int)Type;
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
