using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /// <summary>Find available rooms across all hotels between two dates for a given number of guests.</summary>
    /// <param name="checkIn">Check-in date (yyyy-MM-dd)</param>
    /// <param name="checkOut">Check-out date (yyyy-MM-dd)</param>
    /// <param name="guests">Number of guests</param>
    /// <response code="200">List of available room</response>
    /// <response code="400">Invalid date range or guest count</response>
    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailable([FromQuery] DateTime checkIn,[FromQuery] DateTime checkOut,[FromQuery] int guests,CancellationToken ct)
    {
        var request = new RoomAvailabilityRequest
        {
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guests
        };

        var rooms = await _roomService.GetAvailableRoomsAsync(request, ct);
        return Ok(rooms);
    }
}
