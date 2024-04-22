//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Konyvelo.Data;
//using Konyvelo.Domain;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace Konyvelo.Tests;
//public class ContextTests
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

//    [Test]
//    public async Task Create()
//    {
//        await using var context = new KonyveloDbContext(_options);

//        var model = new Currency()
//        {
//            Code = "HUF"
//        };
//        await context.Currencies.AddAsync(model);
//        await context.SaveChangesAsync();

//        var currencies = await context.Currencies.ToListAsync();

//        Assert.That(currencies, Is.Not.Null);
//        Assert.That(currencies, Is.Not.Empty);
//    }

//    [Test]
//    public async Task CreateAccount()
//    {
//        await using var context = new KonyveloDbContext(_options);

//        var model = new Currency()
//        {
//            Code = "HUF"
//        };
//        await context.Currencies.AddAsync(model);
//        await context.SaveChangesAsync();

//        var currencies = await context.Currencies.ToListAsync();

//        var account = new Account()
//        {
//            //CurrencyId = currencies.First().Id,
//            Currency = currencies.First(),
//            Name = "OTP"
//        };
//        await context.Accounts.AddAsync(account);
//        await context.SaveChangesAsync();

//        var accounts = await context.Accounts.ToListAsync();
        
        
//        Assert.That(accounts, Is.Not.Null);
//        Assert.That(accounts, Is.Not.Empty);
//        Assert.That(accounts[0].Name, Is.EqualTo("OTP"));
//        Assert.That(accounts[0].CurrencyId, Is.EqualTo(1));
//    }
//}
