using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.DAL.Data;
using HelpDeskManager.DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDeskManager.DAL;

public static class ServiceExtensions
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServerConfig(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddSqlServerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, b =>
                b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = true;
            opt.User.RequireUniqueEmail = true;
        }).AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
