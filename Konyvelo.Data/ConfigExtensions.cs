using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konyvelo.Data;
public static class ConfigExtensions
{
    private const string CONNECTION_STRING_KEY = "SqliteConnectionString";
    
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<KonyveloDbContext>(options =>
        {
            options.UseSqlite(configuration[CONNECTION_STRING_KEY]);
        });

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IKonyveloCrudService, KonyveloCrudService>();

        return services;
    }
}
