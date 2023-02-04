using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Crud.Currencies;
using Konyvelo.Logic.Data;
using Konyvelo.Logic.Domain;
using Konyvelo.Logic.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo;

public static class Config
{
    private const string CONNECTION_STRING_KEY = "SqliteConnectionString";
    private const string MIGRATIONS_ASSEMBLY_NAME = "Konyvelo.Logic";

    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<KonyveloDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration[CONNECTION_STRING_KEY], sqliteOptions =>
            {
                sqliteOptions.MigrationsAssembly(MIGRATIONS_ASSEMBLY_NAME);
            });
        });
    }

    public static void ConfigureMediatr(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(typeof(CreateCurrencyCommand));
    }

    public static void ConfigureRepos(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICrudRepo<Currency>, CurrencyRepo>();
        builder.Services.AddScoped<ICrudRepo<Wallet>, WalletRepo>();
        builder.Services.AddScoped<ICrudRepo<Transaction>, TransactionRepo>();
    }

    public static void ConfigureBlazorise(this WebApplicationBuilder builder)
    {
        builder.Services.AddBlazorise();
        builder.Services.AddBootstrapProviders().AddFontAwesomeIcons();
    }
}
