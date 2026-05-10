using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

/// <summary>
/// Test-support endpoints. Not for production use.
/// Allows testers to get the DB into a known state quickly.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Test Data")]
public class SeedController : ControllerBase
{
    private readonly Seeder _seeder;

    public SeedController(Seeder seeder)
    {
        _seeder = seeder;
    }

    /// <summary>
    /// Seed: wipes existing data and populates with test fixtures.
    /// Creates 2 hotels, each with 6 rooms (2 Single, 2 Double, 2 Deluxe).
    /// </summary>
    /// <response code="200">Database seeded successfully</response>
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Seed(CancellationToken ct)
    {
        await _seeder.SeedAsync(ct);
        return Ok(new
        {
            message = "Database seeded successfully.",
            hotels = new[]
            {
                "The Grand Plaza (6 rooms)",
                "Harbour View Hotel (6 rooms)"
            }
        });
    }

    /// <summary>
    /// Reset: removes all bookings, rooms, and hotels.
    /// Call before seeding to start from a clean slate.
    /// </summary>
    /// <response code="204">Database cleared</response>
    [HttpDelete("reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reset(CancellationToken ct)
    {
        await _seeder.ResetAsync(ct);
        return NoContent();
    }
}
