using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>Create a new room booking.</summary>
    /// <response code="201">Booking created, returns booking details with reference number</response>
    /// <response code="400">Invalid request (bad dates, guest count)</response>
    /// <response code="404">Room not found</response>
    /// <response code="409">Room not available for selected dates or capacity exceeded</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request, CancellationToken ct)
    {
        var booking = await _bookingService.CreateBookingAsync(request, ct);        
        return CreatedAtAction(nameof(GetByReference),new { reference = booking.BookingReference },booking);
    }

    /// <summary>Retrieve booking details using a booking reference number.</summary>
    /// <param name="reference">Unique booking reference, e.g. "A3F9B12C"</param>
    /// <response code="200">Booking found</response>
    /// <response code="404">No booking found for this reference</response>
    [HttpGet("{reference}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReference(string reference, CancellationToken ct)
    {
        var booking = await _bookingService.GetByReferenceAsync(reference, ct);

        return booking is null? NotFound(new { detail = $"No booking found with reference '{reference}'." }): Ok(booking);
    }
}
