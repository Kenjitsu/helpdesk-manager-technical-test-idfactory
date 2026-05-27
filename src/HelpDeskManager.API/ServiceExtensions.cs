using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelpDeskManager.API;

public static class ServiceExtensions
{

    public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        return services;
    }

    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Help Desk Manager API",
                Version = "v1",
                Description = "API para la gestion de solictudes realizadas por clientes.",
                Contact = new OpenApiContact
                {
                    Name = "Ricardo Corredor",
                    Email = "ricorga_97@hotmail.com"
                }
            });

            var securityScheme = new OpenApiSecurityScheme()
            {
                Description = "Header de authorización JWT usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            var securityRequirement = new OpenApiSecurityRequirement
           {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "bearerAuth"
                         }
                     },
                     new string[] {}
                 }
             };
            options.AddSecurityDefinition("bearerAuth", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void UseOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();

        }
    }

    public static void AddJwtApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        var jwtSettings = configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>();

        if (jwtSettings == null)
        {
            throw new Exception("JwtSettings not configured.");
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                        ValidateIssuer = true,
                        ValidateAudience = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";

                        var error = new Error("INVALID_TOKEN", "The token provided has expired or its signature is invalid.");
                        var errorResult = Result<string>.Failure(error, HttpStatusCode.Unauthorized, "Access denied");

                        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                        var json = JsonSerializer.Serialize(errorResult, jsonOptions);

                        await context.Response.WriteAsync(json);
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";

                        var result = Result<string>.Failure(
                            new Error("FORBIDDEN", "You do not have permission to access this resource."),
                            HttpStatusCode.Forbidden
                        );

                        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                        });

                        await context.Response.WriteAsync(json);
                    },
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
            .AddPolicy("WriteDataRole", policy => policy.RequireRole("Admin", "Agent"))
            .AddPolicy("ReadDataRole", policy => policy.RequireRole("Admin", "Reader", "Agent"));
    }
}
