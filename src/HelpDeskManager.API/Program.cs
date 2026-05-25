using CustomerManager.BLL;
using HelpDeskManager.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApiServices();
builder.Services.AddBusinessLayerServices(builder.Configuration);

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await AppInitializer.InitializeDatabaseAsync(scope.ServiceProvider);
}

app.Run();
