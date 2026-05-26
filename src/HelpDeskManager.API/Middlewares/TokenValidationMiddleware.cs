using HelpDeskManager.Core.DTOs.Results;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Text.Json;

namespace HelpDeskManager.API.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            var requiresAuth = endpoint.Metadata.GetMetadata<IAuthorizeData>() != null;
            var allowAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;

            if (requiresAuth && !allowAnonymous)
            {
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    await HandleUnauthorizedAsync(context, "MISSING_TOKEN", "The access token is required and was not provided or the format is incorrect.");
                    return;
                }
            }
        }

        await _next(context);
    }

    private static async Task HandleUnauthorizedAsync(HttpContext context, string errorCode, string description)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        var error = new Error(errorCode, description);
        var errorResult = Result<string>.Failure(error, HttpStatusCode.Unauthorized, "Access denied");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(errorResult, options);

        await context.Response.WriteAsync(json);
    }
}
