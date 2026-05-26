using HelpDeskManager.BLL.Services;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.DAL;
using HelpDeskManager.DAL.Data.Seeding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDeskManager.BLL;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataLayerServices(configuration);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();

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
