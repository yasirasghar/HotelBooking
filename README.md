# Hotel Room Booking API

A RESTful hotel room booking API built with **ASP.NET Core 8**, **Entity Framework Core 8** and **SQLite**, following **Clean Architecture**.

## Live API

```
https://hotelbookingapi-b8bsedaue3aye0dw.ukwest-01.azurewebsites.net/
```

Swagger UI is available at the root URL.

\---

## Architecture

Clean Architecture with 4 layers:

* **Domain** — Entities and enums, no dependencies
* **Application** — Business logic, interfaces, DTOs
* **Infrastructure** — EF Core, SQLite, repositories
* **API** — Controllers, middleware, Swagger

\---

## Getting Started

```bash
git clone https://github.com/yasirasghar/HotelBooking.git
cd HotelBooking
dotnet restore
dotnet ef database update --project HotelBooking.Infrastructure --startup-project HotelBooking.API
cd HotelBooking.API \&\& dotnet run
```

\---

## API Endpoints

|Method|Endpoint|Description|
|-|-|-|
|`GET`|`/api/hotels?name={name}`|Find hotel by name|
|`GET`|`/api/rooms/available?checkIn={}\&checkOut={}\&guests={}`|Find available rooms|
|`POST`|`/api/bookings`|Create a booking|
|`GET`|`/api/bookings/{reference}`|Get booking by reference|
|`POST`|`/api/data/seed`|Seed test data|
|`DELETE`|`/api/data/reset`|Reset all data|

\---

## Test Data Setup

**Seed** — creates 2 hotels, each with 6 rooms (2 Single, 2 Double, 2 Deluxe):

```
POST /api/data/seed
```

**Reset** — wipes all data ready for re-seeding:

```
DELETE /api/data/reset
```

\---

## Business Rules

* Hotels have 6 rooms: 2 Single (1 guest), 2 Double (2 guests), 2 Deluxe (4 guests)
* A room cannot be double booked for overlapping dates
* A booking covers the entire stay in one room — no room changes
* Guest count cannot exceed room capacity
* Every booking gets a unique reference number e.g. `A3F9B12C`

\---

## Running Tests

```bash
dotnet test
```

