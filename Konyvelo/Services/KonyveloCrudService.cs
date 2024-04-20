using Microsoft.Data.Sqlite;
using System.Threading;
using Dapper;
using Blazorise;

namespace Konyvelo.Services;

public interface IKonyveloCrudService
{
    Task CreateCurrencyAsync(CreateCurrencyModel model);
    Task CreateAccountAsync(CreateAccountModel model);
    Task<List<GetCurrencyModel>> GetCurrenciesAsync();
    Task<List<GetAccountModel>> GetAccountsAsync();
}

public record CreateCurrencyModel(string Code);

public record GetCurrencyModel
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
}

public record CreateAccountModel(string Name, int CurrencyId);
public class GetAccountModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CurrencyId { get; init; }
}

public class KonyveloCrudService(string dbConnectionString) : IKonyveloCrudService
{
    //public async Task CreateCurrencyAsync(string code)
    //{
    //    if (string.IsNullOrEmpty(code)) { throw new ArgumentNullException(nameof(code)); }

    //    await using var connection = GetConnection();
    //    await connection.ExecuteAsync("insert into currencies (code) values (@CodeValue)", new { CodeValue = code });
    //}

    //public async Task CreateTransactionAsync(CreateTransactionModel transaction, CancellationToken cancellationToken = default)
    //{
    //    if (transaction is null) throw new ArgumentNullException(nameof(transaction));
    //    if (transaction.AccountId < 1) throw new ArgumentOutOfRangeException(nameof(transaction.AccountId));
    //    if (string.IsNullOrEmpty(transaction.Category)) throw new ArgumentException(nameof(transaction.Category));

    //    await using var connection = new SqliteConnection(dbConnectionString);
    //    connection.Open();
    //    await using var command = connection.CreateCommand();
    //    command.CommandText = "insert into transactions (account_id, category, date, info, total) values (@AccountValue, @CategoryValue, @DateValue, @InfoValue, @TotalValue)";

    //    var account_id = command.CreateParameter();
    //    account_id.ParameterName = "AccountValue";
    //    account_id.Value = transaction.AccountId;
    //    command.Parameters.Add(account_id);

    //    var category = command.CreateParameter();
    //    category.ParameterName = "CategoryValue";
    //    category.Value = transaction.Category;
    //    command.Parameters.Add(category);

    //    var date = command.CreateParameter();
    //    date.ParameterName = "DateValue";
    //    date.Value = transaction.Date.ToString("yyyy-MM-dd");
    //    command.Parameters.Add(date);

    //    var info = command.CreateParameter();
    //    info.ParameterName = "InfoValue";
    //    info.Value = transaction.Info;
    //    command.Parameters.Add(info);

    //    var total = command.CreateParameter();
    //    total.ParameterName = "TotalValue";
    //    total.Value = transaction.Total;
    //    command.Parameters.Add(total);

    //    await command.ExecuteNonQueryAsync(cancellationToken);
    //}

    private SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(dbConnectionString);
        connection.Open();

        return connection;
    }

    public async Task CreateCurrencyAsync(CreateCurrencyModel model)
    {
        if (string.IsNullOrEmpty(model.Code)) { throw new ArgumentNullException(nameof(model.Code)); }

        await using var connection = GetConnection();
        await connection.ExecuteAsync("insert into currencies (code) values (@CodeValue)", new { CodeValue = model.Code });
    }

    public async Task CreateAccountAsync(CreateAccountModel model)
    {
        if (string.IsNullOrEmpty(model.Name)) { throw new ArgumentNullException(nameof(model.Name)); }
        if (model.CurrencyId < 1) { throw new ArgumentException(nameof(model.CurrencyId)); }

        await using var connection = GetConnection();
        var walletExists = await connection.QueryFirstAsync<int>("select exists (select id from currencies where id = @Id)", new { Id = model.CurrencyId });
        if (walletExists == 0)
        {
            throw new ArgumentException(nameof(model.CurrencyId));
        }

        await connection.ExecuteAsync("insert into accounts (currency_id, name) values (@CurrencyId, @Name)", new { CurrencyId = model.CurrencyId, Name = model.Name });
    }

    public async Task<List<GetCurrencyModel>> GetCurrenciesAsync()
    {
        await using var connection = GetConnection();

        return (await connection.QueryAsync<GetCurrencyModel>("select * from currencies")).ToList();
    }

    public async Task<List<GetAccountModel>> GetAccountsAsync()
    {
        await using var connection = GetConnection();

        return (await connection.QueryAsync<GetAccountModel>("select id, name, currency_id as CurrencyId from accounts")).ToList();
    }
}
