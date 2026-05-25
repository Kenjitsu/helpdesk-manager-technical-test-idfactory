using HelpDeskManager.DAL;
using HelpDeskManager.DAL.Data.Seeding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManager.BLL;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataLayerServices(configuration);

        return services;
    }

}

public static class AppInitializer
{
    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        await DatabaseSeeder.SeedAsync(serviceProvider);
    }
}
