//using Dapper;
//using Konyvelo.Services;
//using Microsoft.Data.Sqlite;

//namespace Konyvelo.Tests;

//public class Tests
//{
//    private const string Path = "D:\\personal\\penz\\konyvelo_test.sqlite";
//    private const string ConnectionString = $"Data Source={Path}";

//    [SetUp]
//    public async Task Setup()
//    {
//        await using var connection = GetConnection();

//        await connection.ExecuteAsync("delete from transactions");
//        await connection.ExecuteAsync("delete from accounts");
//        await connection.ExecuteAsync("delete from currencies");
//        await connection.ExecuteAsync("delete from sqlite_sequence");
//    }

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

//        await using var connection = GetConnection();
//        var sql = await File.ReadAllTextAsync(@"D:\repos\Konyvelo\Konyvelo\db.sql");
//        await connection.ExecuteAsync(sql);
//    }

//    [Test]
//    public async Task CreateCurrency_ShouldCreate()
//    {
//        // arrange
//        var service = new KonyveloCrudService(ConnectionString);

//        // act
//        await service.CreateCurrencyAsync(new CreateCurrencyModel("HUF"));

//        // assert
//        var currencies = await service.GetCurrenciesAsync();
//        Assert.That(currencies, Is.Not.Null);
//        Assert.That(currencies.Count, Is.EqualTo(1));
//        Assert.That(currencies[0].Code, Is.EqualTo("HUF"));
//    }

//    [Test]
//    public async Task CreateCurrencyAndAccount_ShouldCreate()
//    {
//        // arrange
//        var service = new KonyveloCrudService(ConnectionString);
//        await service.CreateCurrencyAsync(new CreateCurrencyModel("HUF"));
//        var currencies = await service.GetCurrenciesAsync();

//        // act
//        await service.CreateAccountAsync(new CreateAccountModel("OTP", currencies.First().Id));

//        // assert
//        var accounts = await service.GetAccountsAsync();
//        Assert.That(accounts, Is.Not.Null);
//        Assert.That(accounts.Count, Is.EqualTo(1));
//        Assert.That(accounts[0].Name, Is.EqualTo("OTP"));
//        Assert.That(accounts[0].CurrencyId, Is.EqualTo(currencies.First().Id));
//    }

//    [Test]
//    public async Task CreateCurrencyAndAccountCode_ShouldCreate()
//    {
//        // arrange
//        var service = new KonyveloCrudService(ConnectionString);
//        await service.CreateCurrencyAsync(new CreateCurrencyModel("HUF"));
//        var currencies = await service.GetCurrenciesAsync();

//        // act
//        await service.CreateAccountAsync(new CreateAccountModel("OTP", currencies.First().Id));

//        // assert
//        var accounts = await service.GetAccountsWithCurrency();
//        Assert.That(accounts, Is.Not.Null);
//        Assert.That(accounts.Count, Is.EqualTo(1));
//        Assert.That(accounts[0].Name, Is.EqualTo("OTP"));
//        Assert.That(accounts[0].CurrencyId, Is.EqualTo(currencies.First().Id));
//        Assert.That(accounts[0].Code, Is.EqualTo(currencies.First().Code));
//    }

//    [Test]
//    public async Task CreateTransaction_ShouldCreate()
//    {
//        // arrange
//        var service = new KonyveloCrudService(ConnectionString);
//        await service.CreateCurrencyAsync(new CreateCurrencyModel("HUF"));
//        var currencies = await service.GetCurrenciesAsync();
//        await service.CreateAccountAsync(new CreateAccountModel("OTP", currencies.First().Id));
//        var accounts = await service.GetAccountsWithCurrency();

//        // act
//        await service.CreateTransactionAsync(new CreateTransactionModel(accounts.First().Id, "kaja", -1500, DateOnly.FromDateTime(DateTime.Today), "kínai"));
//        await service.CreateTransactionAsync(new CreateTransactionModel(accounts.First().Id, "egyéb", -2000, DateOnly.FromDateTime(DateTime.Today), null));

//        var transactions = await service.GetTransactions();
//        Assert.That(transactions, Is.Not.Null);
//        Assert.That(transactions.Count, Is.EqualTo(2));
//        Assert.That(transactions[0].Category, Is.EqualTo("kaja"));
//        Assert.That(transactions[0].Total, Is.EqualTo(-1500));
//        Assert.That(transactions[0].Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.Today)));
//        Assert.That(transactions[0].Info, Is.EqualTo("kínai"));
//        Assert.That(transactions[1].Category, Is.EqualTo("egyéb"));
//        Assert.That(transactions[1].Total, Is.EqualTo(-2000));
//        Assert.That(transactions[1].Date, Is.EqualTo(DateOnly.FromDateTime(DateTime.Today)));
//        Assert.That(transactions[1].Info, Is.Null);
//    }

//    protected virtual SqliteConnection GetConnection()
//    {
//        var connection = new SqliteConnection(ConnectionString);
//        connection.Open();

//        return connection;
//    }
//}

//public class CodeModel
//{
//    public string Code { get; set; }
//}