using HelpDeskManager.Core.DTOs.Results;
using System.Net;
using System.Text.Json;

namespace HelpDeskManager.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    private readonly Dictionary<int, (string, string)> _errorMappings;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
        _errorMappings = new Dictionary<int, (string Code, string Message)>
        {
            { StatusCodes.Status401Unauthorized, (Code: "UNAUTHORIZED", Message: "Unauthorized access") },
            { StatusCodes.Status404NotFound, (Code: "NOT_FOUND", Message: "Resource not found") },
            { StatusCodes.Status408RequestTimeout, (Code: "REQUEST_TIMEOUT", Message: "Request timed out") },
            { StatusCodes.Status500InternalServerError, (Code: "INTERNAL_SERVER_ERROR", Message: "An internal server error occurred") }
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            KeyNotFoundException => StatusCodes.Status404NotFound,

            TimeoutException => StatusCodes.Status408RequestTimeout,

            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = _environment.IsDevelopment()
                ? Result<object>.Failure(new Error(_errorMappings[statusCode].Item1, exception.StackTrace?.ToString()), (HttpStatusCode)statusCode, exception.Message)
                : Result<object>.Failure(new Error(_errorMappings[statusCode].Item1, _errorMappings[statusCode].Item2), (HttpStatusCode)statusCode);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}
