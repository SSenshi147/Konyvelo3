using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

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
        SqlMapper.AddTypeHandler(new DapperSqliteDateOnlyTypeHandler());

        return services;
    }
}

public class DapperSqliteDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date)
        => parameter.Value = date.ToString("yyyy-MM-dd");

    public override DateOnly Parse(object value)
        => DateOnly.Parse(value.ToString());
}