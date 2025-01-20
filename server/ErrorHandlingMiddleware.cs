using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string message = "An unexpected error occurred.";

        if (exception is DbUpdateException)
        {
            statusCode = HttpStatusCode.BadRequest;
            message = "A database update error occurred.";
        }
        else if (exception is InvalidOperationException)
        {
            statusCode = HttpStatusCode.BadRequest;
            message = "Invalid operation.";
        }

        response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            StatusCode = response.StatusCode,
            Message = message
        };

        var errorJson = JsonSerializer.Serialize(errorResponse);

        return response.WriteAsync(errorJson);
    }
}
