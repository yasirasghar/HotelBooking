using System.Net;
using System.Text.Json;

namespace HotelBooking.API.Middleware;

/// <summary>
/// Catches unhandled exceptions
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid Request"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Not Found"),
            InvalidOperationException => (HttpStatusCode.Conflict, "Business Rule Violation"),
            _ => (HttpStatusCode.InternalServerError, "Server Error")
        };

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)statusCode}",
            title,
            status = (int)statusCode,
            detail = exception.Message,
            traceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}
