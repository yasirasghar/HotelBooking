using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Moq;

namespace HotelBooking.Tests.Unit;

/// <summary>
/// Unit tests for BookingService.
/// </summary>
public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepo = new();
    private readonly Mock<IRoomRepository> _roomRepo = new();
    private BookingService CreateSut() => new(_bookingRepo.Object, _roomRepo.Object);

    private static Room DoubleRoom(int id = 1) => new()
    {
        Id = id,
        Type = RoomType.Double,   // capacity 2
        HotelId = 1,
        Hotel = new Hotel { Id = 1, Name = "Test Hotel", Address = "" }
    };

    
    [Fact]
    public async Task CreateBookingAsync_ValidRequest_ReturnsBookingResponse()
    {
        // Arrange
        var checkIn = DateTime.UtcNow.Date.AddDays(1);
        var checkOut = checkIn.AddDays(3);
        var room = DoubleRoom();

        _roomRepo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(room);
        _bookingRepo.Setup(r => r.HasOverlappingBookingAsync(1, checkIn, checkOut, default)).ReturnsAsync(false);
        _bookingRepo.Setup(r => r.ReferenceExistsAsync(It.IsAny<string>(), default)).ReturnsAsync(false);
        _bookingRepo.Setup(r => r.CreateAsync(It.IsAny<Booking>(), default))
            .ReturnsAsync((Booking b, CancellationToken _) =>
            {
                b.Id = 99;
                b.Room = room;
                return b;
            });

        var sut = CreateSut();
        var request = new BookingRequest
        {
            RoomId = 1,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = 2,
            GuestName = "Jane Doe"
        };

        // Act
        var result = await sut.CreateBookingAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jane Doe", result.GuestName);
        Assert.Equal(3, result.Nights);
        Assert.NotEmpty(result.BookingReference);
    }

    // Business rule: capacity

    [Fact]
    public async Task CreateBookingAsync_GuestCountExceedsCapacity_ThrowsInvalidOperation()
    {
        _roomRepo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(DoubleRoom());

        var sut = CreateSut();
        var request = new BookingRequest
        {
            RoomId = 1,
            CheckIn = DateTime.UtcNow.Date.AddDays(1),
            CheckOut = DateTime.UtcNow.Date.AddDays(3),
            GuestCount = 5,   // Double room capacity = 2
            GuestName = "Big Group"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.CreateBookingAsync(request));
    }

    // Business rule: no double booking 

    [Fact]
    public async Task CreateBookingAsync_OverlappingBookingExists_ThrowsInvalidOperation()
    {
        var checkIn = DateTime.UtcNow.Date.AddDays(1);
        var checkOut = checkIn.AddDays(3);

        _roomRepo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(DoubleRoom());
        _bookingRepo.Setup(r => r.HasOverlappingBookingAsync(1, checkIn, checkOut, default)).ReturnsAsync(true);

        var sut = CreateSut();
        var request = new BookingRequest
        {
            RoomId = 1,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = 1,
            GuestName = "John"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.CreateBookingAsync(request));
    }

    // Input validation

    [Fact]
    public async Task CreateBookingAsync_CheckOutBeforeCheckIn_ThrowsArgumentException()
    {
        var sut = CreateSut();
        var request = new BookingRequest
        {
            RoomId = 1,
            CheckIn = DateTime.UtcNow.Date.AddDays(5),
            CheckOut = DateTime.UtcNow.Date.AddDays(2),   // before check-in
            GuestCount = 1,
            GuestName = "Alice"
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => sut.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_CheckInInPast_ThrowsArgumentException()
    {
        var sut = CreateSut();
        var request = new BookingRequest
        {
            RoomId = 1,
            CheckIn = DateTime.UtcNow.Date.AddDays(-1),  // yesterday
            CheckOut = DateTime.UtcNow.Date.AddDays(2),
            GuestCount = 1,
            GuestName = "Bob"
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => sut.CreateBookingAsync(request));
    }

    // Reference lookup

    [Fact]
    public async Task GetByReferenceAsync_UnknownReference_ReturnsNull()
    {
        _bookingRepo.Setup(r => r.GetByReferenceAsync("UNKNOWN", default)).ReturnsAsync((Booking?)null);

        var sut = CreateSut();
        var result = await sut.GetByReferenceAsync("UNKNOWN");

        Assert.Null(result);
    }
}
