using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Konyvelo.Data;
using Konyvelo.Services;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo;

public static class Config
{
    private const string CONNECTION_STRING_KEY = "SqliteConnectionString";

    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<KonyveloDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration[CONNECTION_STRING_KEY]);
        });
    }

    public static void ConfigureBlazorise(this WebApplicationBuilder builder)
    {
        builder.Services.AddBlazorise();
        builder.Services.AddBootstrapProviders().AddFontAwesomeIcons();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<KonyveloService>();
    }
}
