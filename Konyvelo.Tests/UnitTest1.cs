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

        await connection.ExecuteAsync("DELETE FROM currencies");
        await connection.ExecuteAsync("delete from sqlite_sequence");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        if (!File.Exists(Path))
        {
            await using var _ = File.Create(Path);
        }

        await using var connection = GetConnection();

        await connection.ExecuteAsync("create table if not exists 'currencies' ('Id' INTEGER NOT NULL CONSTRAINT 'PK_NewCurrencies' PRIMARY KEY AUTOINCREMENT, 'Code' TEXT NOT NULL);");
    }

    [Test]
    public async Task Test1()
    {
        var service = new KonyveloCrudService(ConnectionString);
        await service.CreateCurrencyAsync("HUF");

        await using var connection = GetConnection();

        var queryResult = (await connection.QueryAsync<CodeModel>("select code from currencies")).ToArray();
        Assert.That(queryResult, Is.Not.Null);
        Assert.That(queryResult.Length, Is.EqualTo(1));
        Assert.That(queryResult[0].Code, Is.EqualTo("HUF"));
    }

    [Test]
    public async Task Test2()
    {
        var service = new KonyveloCrudService(ConnectionString);
        await service.CreateCurrencyAsync("HUF");
        await service.CreateCurrencyAsync("USD");
        await service.CreateCurrencyAsync("EUR");

        await using var connection = GetConnection();

        var queryResult = (await connection.QueryAsync<CodeModel>("select code from currencies")).ToArray();
        Assert.That(queryResult, Is.Not.Null);
        Assert.That(queryResult.Length, Is.EqualTo(3));
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