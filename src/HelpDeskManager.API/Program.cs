using HelpDeskManager.API;
using HelpDeskManager.API.Middlewares;
using HelpDeskManager.BLL;

var builder = WebApplication.CreateBuilder(args);

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

public partial class Program { }
