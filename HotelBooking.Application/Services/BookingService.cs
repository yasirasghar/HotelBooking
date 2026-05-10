using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository,IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<BookingResponse> CreateBookingAsync(BookingRequest request,CancellationToken ct = default)
        {
            // Input validation
            if (request.CheckIn.Date >= request.CheckOut.Date)
                throw new ArgumentException("Check-out must be after check-in.");

            if (request.CheckIn.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Check-in cannot be in the past.");

            if (request.GuestCount < 1)
                throw new ArgumentException("Guest count must be at least 1.");

            // Load room, verify it exists
            var room = await _roomRepository.GetByIdAsync(request.RoomId, ct)
                ?? throw new KeyNotFoundException($"Room {request.RoomId} not found.");

            // Business rule: capacity
            if (request.GuestCount > room.Capacity)
                throw new InvalidOperationException(
                    $"Room capacity is {room.Capacity}. Cannot accommodate {request.GuestCount} guests.");

            // Business rule: no double booking
            var isOverlapping = await _bookingRepository.HasOverlappingBookingAsync(request.RoomId,request.CheckIn.Date,request.CheckOut.Date,ct);

            if (isOverlapping)
                throw new InvalidOperationException(
                    "Room is not available for the selected dates.");

            // Generate unique booking reference 
            string reference;
            do
            {
                reference = GenerateReference();
            }
            while (await _bookingRepository.ReferenceExistsAsync(reference, ct));

            // --- Persist ---
            var booking = new Booking
            {
                BookingReference = reference,
                GuestName = request.GuestName,
                CheckIn = request.CheckIn.Date,
                CheckOut = request.CheckOut.Date,
                GuestCount = request.GuestCount,
                RoomId = request.RoomId
            };

            var created = await _bookingRepository.CreateAsync(booking, ct);

            return MapToResponse(created, room);
        }

        public async Task<BookingResponse?> GetByReferenceAsync(string reference,CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(reference))
                throw new ArgumentException("Booking reference cannot be empty.");

            var booking = await _bookingRepository.GetByReferenceAsync(reference.ToUpper(), ct);

            if (booking is null)
                return null;

            return MapToResponse(booking, booking.Room);
        }

        // helpers

        private static string GenerateReference()
            => Guid.NewGuid().ToString("N")[..8].ToUpper();

        private static BookingResponse MapToResponse(Booking booking, Room room) => new()
        {
            Id = booking.Id,
            BookingReference = booking.BookingReference,
            GuestName = booking.GuestName,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            GuestCount = booking.GuestCount,
            HotelName = room.Hotel?.Name ?? string.Empty,
            Room = new RoomDto
            {
                Id = room.Id,
                RoomType = room.Type.ToString(),
                Capacity = room.Capacity,
                HotelId = room.HotelId
            }
        };
    }
}
