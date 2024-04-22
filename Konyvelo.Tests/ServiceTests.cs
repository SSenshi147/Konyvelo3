using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazorise.Extensions;
using Dapper;
using Konyvelo.Data;
using Konyvelo.Data.Dtos;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Konyvelo.Tests;
public class ServiceTests
{
    private const string Path = "D:\\personal\\penz\\konyvelo_test.sqlite";
    private const string ConnectionString = $"Data Source={Path}";

    private DbContextOptions<KonyveloDbContext> _options;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        if (!File.Exists(Path))
        {
            await using var _ = File.Create(Path);
        }
        else
        {
            File.Delete(Path);
        }

        var builder = new DbContextOptionsBuilder<KonyveloDbContext>();
        builder.UseSqlite(ConnectionString);
        _options = builder.Options;

        await using var connection = GetConnection();
        var sql = await File.ReadAllTextAsync(@"D:\repos\Konyvelo\Konyvelo\db.sql");
        await connection.ExecuteAsync(sql);
    }

    [SetUp]
    public async Task Setup()
    {
        await using var context = new KonyveloDbContext(_options);

        await context.Transactions.ExecuteDeleteAsync();
        await context.Accounts.ExecuteDeleteAsync();
        await context.Currencies.ExecuteDeleteAsync();
    }

    private IKonyveloCrudService GetService()
    {
        return new KonyveloCrudService(new KonyveloDbContext(_options), ConnectionString);
    }

    protected virtual SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        return connection;
    }

    [Test]
    public async Task CreateCurrency()
    {
        var service = GetService();

        await service.CreateCurrencyAsync(new CreateCurrencyDto()
        {
            Code = "HUF"
        });

        var currencies = await service.GetAllCurrenciesAsync();
        Assert.That(currencies, Is.Not.Null);
        Assert.That(currencies, Is.Not.Empty);
        Assert.That(currencies[0].Code, Is.EqualTo("HUF"));
        Assert.That(currencies[0].Total, Is.EqualTo(0));
    }

    [Test]
    public async Task CreateAccount()
    {
        var service = GetService();
        await service.CreateCurrencyAsync(new CreateCurrencyDto()
        {
            Code = "HUF"
        });
        var currencies = await service.GetAllCurrenciesAsync();

        await service.CreateAccountAsync(new CreateAccountDto()
        {
            CurrencyId = currencies[0].Id,
            Name = "OTP"
        });

        var accounts = await service.GetAllAccountsAsync();
        Assert.That(accounts, Is.Not.Null);
        Assert.That(accounts, Is.Not.Empty);
        Assert.That(accounts[0].Name, Is.EqualTo("OTP"));
        Assert.That(accounts[0].CurrencyCode, Is.EqualTo("HUF"));
        Assert.That(accounts[0].Total, Is.EqualTo(0));
    }

    [Test]
    public async Task CreateTransaction()
    {
        var service = GetService();
        await service.CreateCurrencyAsync(new CreateCurrencyDto()
        {
            Code = "HUF"
        });
        var currencies = await service.GetAllCurrenciesAsync();
        await service.CreateAccountAsync(new CreateAccountDto()
        {
            CurrencyId = currencies[0].Id,
            Name = "OTP"
        });
        var accounts = await service.GetAllAccountsAsync();

        await service.CreateTransactionAsync(new CreateTransactionDto()
        {
            AccountId = accounts[0].Id,
            Category = "kaja",
            Date = DateOnly.FromDateTime(DateTime.Today),
            Info = "pizza",
            Total = -3000
        });

        var transactions = await service.GetAllTransactionsAsync();
        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions, Is.Not.Empty);
        Assert.That(transactions[0].AccountName, Is.EqualTo("OTP"));
        Assert.That(transactions[0].CurrencyCode, Is.EqualTo("HUF"));
        Assert.That(transactions[0].Total, Is.EqualTo(-3000));
        Assert.That(transactions[0].Category, Is.EqualTo("kaja"));
        Assert.That(transactions[0].Info, Is.EqualTo("pizza"));
        Assert.That(transactions[0].Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.Today)));
    }
}
