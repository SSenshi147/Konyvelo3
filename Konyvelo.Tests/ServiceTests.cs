//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Konyvelo.Data;
//using Konyvelo.Data.Dtos;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.Extensions.Configuration;

//namespace Konyvelo.Tests;
//public class ServiceTests
//{
//    private const string Path = "D:\\personal\\penz\\konyvelo_test.sqlite";
//    private const string ConnectionString = $"Data Source={Path}";

//    private DbContextOptions<KonyveloDbContext> _options;

//    [OneTimeSetUp]
//    public async Task OneTimeSetup()
//    {
//        if (!File.Exists(Path))
//        {
//            await using var _ = File.Create(Path);
//        }
//        else
//        {
//            File.Delete(Path);
//        }

//        var builder = new DbContextOptionsBuilder<KonyveloDbContext>();
//        builder.UseSqlite(ConnectionString);
//        _options = builder.Options;

//        await using var context = new KonyveloDbContext(_options);

//        if ((await context.Database.GetPendingMigrationsAsync()).Any())
//        {
//            await context.Database.MigrateAsync();
//        }
//    }

//    [SetUp]
//    public async Task Setup()
//    {
//        await using var context = new KonyveloDbContext(_options);

//        await context.Transactions.ExecuteDeleteAsync();
//        await context.Accounts.ExecuteDeleteAsync();
//        await context.Currencies.ExecuteDeleteAsync();
//    }

//    private IKonyveloCrudService GetService()
//    {
//        using var context = new KonyveloDbContext(_options);
//        return new KonyveloCrudService(context);
//    }

//    [Test]
//    public async Task CreateCurrency()
//    {
//        var service = GetService();

//        await service.CreateCurrencyAsync(new CreateCurrencyDto()
//        {
//            Code = "HUF"
//        });

//        var currencies = await service.GetAllCurrenciesAsync();
//        Assert.That(currencies, Is.Not.Null);
//        Assert.That(currencies, Is.Not.Empty);
//        Assert.That(currencies[0].Code, Is.EqualTo("HUF"));
//    }
//}
