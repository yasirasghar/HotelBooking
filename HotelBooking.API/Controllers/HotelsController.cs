using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    /// <summary>Find a hotel by name (partial match, case-insensitive).</summary>
    /// <param name="name">Full or partial hotel name</param>
    /// <response code="200">Hotel found</response>
    /// <response code="404">No hotel matched the given name</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest(new { detail = "Query parameter 'name' is required." });

        var hotel = await _hotelService.FindByNameAsync(name, ct);

        return hotel is null ? NotFound(new { detail = $"No hotel found matching '{name}'." }) : Ok(hotel);
    }
}
