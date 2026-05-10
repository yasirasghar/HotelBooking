using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int HotelId { get; set; }
    }
}
