using System.Collections;
using Konyvelo.Logic.Data;
using Konyvelo.Logic.Dtos;
using Konyvelo.Logic.Exceptions;
using Konyvelo.Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Tests;

public class ServiceTests
{
    private const string Path = "D:\\personal\\penz\\konyvelo_test.sqlite";
    private const string ConnectionString = $"Data Source={Path}";

    private DbContextOptions<KonyveloDbContext> _options;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        {
            await using var _ = File.Create(Path);
        }

        var builder = new DbContextOptionsBuilder<KonyveloDbContext>();
        builder.UseSqlite(ConnectionString);
        _options = builder.Options;

        await using var context = new KonyveloDbContext(_options);
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    [SetUp]
    public async Task Setup()
    {
        await using var context = new KonyveloDbContext(_options);

        await context.Transactions.ExecuteDeleteAsync();
        await context.Accounts.ExecuteDeleteAsync();
        await context.Currencies.ExecuteDeleteAsync();
    }

    private KonyveloService GetService()
    {
        return new KonyveloService(new KonyveloDbContext(_options));
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

    [TestCaseSource(nameof(CreateCurrencyShouldThrowData))]
    public void CreateCurrency_ShouldThrow(CreateCurrencyDto dto, Type exceptionType)
    {
        var service = GetService();
        Assert.ThrowsAsync(exceptionType, async () => await service.CreateCurrencyAsync(dto));
    }

    public static IEnumerable CreateCurrencyShouldThrowData
    {
        get
        {
            yield return new TestCaseData(null, typeof(ArgumentNullException));
            yield return new TestCaseData(new CreateCurrencyDto(), typeof(ArgumentException));
        }
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

    [TestCaseSource(nameof(CreateAccountShouldThrowData))]
    public void CreateAccount_ShouldThrow(CreateAccountDto dto, Type exceptionType)
    {
        var service = GetService();
        Assert.ThrowsAsync(exceptionType, async () => await service.CreateAccountAsync(dto));
    }

    public static IEnumerable CreateAccountShouldThrowData
    {
        get
        {
            yield return new TestCaseData(null, typeof(ArgumentNullException));
            yield return new TestCaseData(new CreateAccountDto(), typeof(ArgumentException));
            yield return new TestCaseData(new CreateAccountDto()
            {
                Name = "asd",
                CurrencyId = 1
            }, typeof(NotFoundException));
        }
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
            Total = (decimal)-3000.56
        });

        var transactions = await service.GetAllTransactionsAsync();
        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions, Is.Not.Empty);
        Assert.That(transactions[0].AccountName, Is.EqualTo("OTP"));
        Assert.That(transactions[0].CurrencyCode, Is.EqualTo("HUF"));
        Assert.That(transactions[0].Total, Is.EqualTo(-3000.56));
        Assert.That(transactions[0].Category, Is.EqualTo("kaja"));
        Assert.That(transactions[0].Info, Is.EqualTo("pizza"));
        Assert.That(transactions[0].Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.Today)));
    }

    [TestCaseSource(nameof(CreateTransactionShouldThrowData))]
    public void CreateTransaction_ShouldThrow(CreateTransactionDto dto, Type exceptionType)
    {
        var service = GetService();
        Assert.ThrowsAsync(exceptionType, async () => await service.CreateTransactionAsync(dto));
    }

    public static IEnumerable CreateTransactionShouldThrowData
    {
        get
        {
            yield return new TestCaseData(null, typeof(ArgumentNullException));
            yield return new TestCaseData(new CreateTransactionDto(), typeof(ArgumentException));
            yield return new TestCaseData(new CreateTransactionDto()
            {
                Category = "asd",
                AccountId = 1
            }, typeof(NotFoundException));
        }
    }

    [Test]
    public async Task UpdateCurrency()
    {
        var service = GetService();
        await service.CreateCurrencyAsync(new CreateCurrencyDto()
        {
            Code = "HUF"
        });
        var currencies = await service.GetAllCurrenciesAsync();

        await service.UpdateCurrencyAsync(new UpdateCurrencyDto()
        {
            Code = "USD",
            Id = currencies[0].Id,
        });

        currencies = await service.GetAllCurrenciesAsync();
        Assert.That(currencies, Is.Not.Null);
        Assert.That(currencies, Is.Not.Empty);
        Assert.That(currencies[0].Code, Is.EqualTo("USD"));
        Assert.That(currencies[0].Total, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateAccount()
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

        await service.UpdateAccountAsync(new UpdateAccountDto()
        {
            Id = accounts[0].Id,
            Name = "Revolut"
        });

        accounts = await service.GetAllAccountsAsync();
        Assert.That(accounts, Is.Not.Null);
        Assert.That(accounts, Is.Not.Empty);
        Assert.That(accounts[0].Name, Is.EqualTo("Revolut"));
        Assert.That(accounts[0].CurrencyCode, Is.EqualTo("HUF"));
        Assert.That(accounts[0].Total, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateTransaction()
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

        await service.UpdateTransactionAsync(new UpdateTransactionDto()
        {
            Category = "telenor",
            Info = "számla",
            Total = -5773,
            Id = transactions[0].Id
        });

        transactions = await service.GetAllTransactionsAsync();
        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions, Is.Not.Empty);
        Assert.That(transactions[0].AccountName, Is.EqualTo("OTP"));
        Assert.That(transactions[0].CurrencyCode, Is.EqualTo("HUF"));
        Assert.That(transactions[0].Total, Is.EqualTo(-5773));
        Assert.That(transactions[0].Category, Is.EqualTo("telenor"));
        Assert.That(transactions[0].Info, Is.EqualTo("számla"));
        Assert.That(transactions[0].Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.Today)));
    }

    [Test]
    public async Task DeleteCurrency()
    {
        var service = GetService();
        await service.CreateCurrencyAsync(new CreateCurrencyDto()
        {
            Code = "HUF"
        });
        var currencies = await service.GetAllCurrenciesAsync();

        await service.DeleteCurrencyAsync(currencies[0].Id);

        currencies = await service.GetAllCurrenciesAsync();
        Assert.That(currencies, Is.Not.Null);
        Assert.That(currencies, Is.Empty);
    }

    [Test]
    public async Task DeleteAccount()
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

        await service.DeleteAccountAsync(accounts[0].Id);

        accounts = await service.GetAllAccountsAsync();
        Assert.That(accounts, Is.Not.Null);
        Assert.That(accounts, Is.Empty);
    }

    [Test]
    public async Task DeleteTransaction()
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

        await service.DeleteTransactionAsync(transactions[0].Id);

        transactions = await service.GetAllTransactionsAsync();
        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions, Is.Empty);
    }
}