using Microsoft.OpenApi.Models;

namespace HelpDeskManager.API;

public static class ServiceExtensions
{
    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
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
}
