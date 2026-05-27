using HelpDeskManager.API;
using HelpDeskManager.API.Middlewares;
using HelpDeskManager.BLL;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, serviceProvider, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddConfiguredControllers();
    builder.Services.AddOpenApiServices();
    builder.Services.AddJwtApiServices(builder.Configuration);

    builder.Services.AddBusinessLayerServices(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<TokenValidationMiddleware>();
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseOpenApi();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        await AppInitializer.InitializeDatabaseAsync(scope.ServiceProvider);
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }


