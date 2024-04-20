using Dapper;
using Konyvelo.Services;
using Microsoft.Data.Sqlite;

namespace Konyvelo.Tests;

public class Tests
{
    private const string Path = "D:\\personal\\penz\\konyvelo_test.sqlite";
    private const string ConnectionString = $"Data Source={Path}";

    [SetUp]
    public async Task Setup()
    {
        await using var connection = GetConnection();

        await connection.ExecuteAsync("delete from transactions");
        await connection.ExecuteAsync("delete from accounts");
        await connection.ExecuteAsync("delete from currencies");
        await connection.ExecuteAsync("delete from sqlite_sequence");
    }

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

        await using var connection = GetConnection();

        await connection.ExecuteAsync("create table if not exists 'currencies' ('id' INTEGER NOT NULL CONSTRAINT 'PK_currencies' PRIMARY KEY AUTOINCREMENT, 'code' TEXT NOT NULL);");

        await connection.ExecuteAsync("CREATE TABLE if not exists 'accounts'" +
                                      "('id' INTEGER NOT NULL CONSTRAINT 'PK_accounts' PRIMARY KEY AUTOINCREMENT," +
                                      " 'name' TEXT NOT NULL, " +
                                      " 'currency_id' INTEGER NOT NULL, CONSTRAINT 'FK_accounts_currencies_currency_id' FOREIGN KEY ('currency_id') REFERENCES 'currencies' ('id') ON DELETE CASCADE)");

        await connection.ExecuteAsync("CREATE TABLE 'transactions' (" +
            "'id' INTEGER NOT NULL CONSTRAINT 'PK_transactions' PRIMARY KEY AUTOINCREMENT," +
            "'type' TEXT NOT NULL," +
            "'info' TEXT NOT NULL," +
            "'date' TEXT NOT NULL," +
            "'account_id' INTEGER NOT NULL," +
            "CONSTRAINT 'FK_transactions_accounts_account_id' FOREIGN KEY ('account_id') REFERENCES 'accounts' ('id') ON DELETE CASCADE)");
    }

    [Test]
    public async Task CreateCurrency_ShouldCreate()
    {
        // arrange
        var service = new KonyveloCrudService(ConnectionString);

        // act
        await service.CreateCurrencyAsync(new CreateCurrencyModel("HUF"));

        // assert
        var currencies = await service.GetCurrenciesAsync();
        Assert.That(currencies, Is.Not.Null);
        Assert.That(currencies.Count, Is.EqualTo(1));
        Assert.That(currencies[0].Code, Is.EqualTo("HUF"));
    }

    [Test]
    public async Task CreateCurrencyAndAccount_ShouldCreate()
    {
        // arrange
        var service = new KonyveloCrudService(ConnectionString);
        await service.CreateCurrencyAsync(new CreateCurrencyModel("HUF"));
        var currencies = await service.GetCurrenciesAsync();

        // act
        await service.CreateAccountAsync(new CreateAccountModel("OTP", currencies.First().Id));

        // assert
        var accounts = await service.GetAccountsAsync();
        Assert.That(accounts, Is.Not.Null);
        Assert.That(accounts.Count, Is.EqualTo(1));
        Assert.That(accounts[0].Name, Is.EqualTo("OTP"));
        Assert.That(accounts[0].CurrencyId, Is.EqualTo(currencies.First().Id));
    }

    protected virtual SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        return connection;
    }
}

public class CodeModel
{
    public string Code { get; set; }
}