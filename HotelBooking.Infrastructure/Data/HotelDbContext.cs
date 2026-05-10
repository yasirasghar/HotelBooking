using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HotelBooking.Infrastructure.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Hotel>(e =>
        {
            e.HasKey(h => h.Id);
            e.Property(h => h.Name)
             .IsRequired()
             .HasMaxLength(200);
            e.HasIndex(h => h.Name)
             .IsUnique();                         
            e.Property(h => h.Address)
             .HasMaxLength(500);
        });

        
        modelBuilder.Entity<Room>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Type)
             .IsRequired()
             .HasConversion<string>();
            
            e.Ignore(r => r.Capacity);

            e.HasOne(r => r.Hotel)
             .WithMany(h => h.Rooms)
             .HasForeignKey(r => r.HotelId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        
        modelBuilder.Entity<Booking>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.BookingReference)
             .IsRequired()
             .HasMaxLength(20);
            e.HasIndex(b => b.BookingReference)
             .IsUnique();                         
            e.Property(b => b.GuestName)
             .IsRequired()
             .HasMaxLength(200);
            e.Property(b => b.CheckIn)
             .IsRequired();
            e.Property(b => b.CheckOut)
             .IsRequired();
            e.Property(b => b.GuestCount)
             .IsRequired();

            e.HasOne(b => b.Room)
             .WithMany(r => r.Bookings)
             .HasForeignKey(b => b.RoomId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
