using Microsoft.Data.Sqlite;
using System.Threading;
using Dapper;

namespace Konyvelo.Services;

public class KonyveloCrudService(string dbConnectionString)
{
    public async Task CreateCurrencyAsync(string code)
    {
        if (string.IsNullOrEmpty(code)) { throw new ArgumentNullException(nameof(code)); }
        
        await using var connection = GetConnection();
        await connection.ExecuteAsync("insert into currencies (code) values (@CodeValue)", new { CodeValue = code });
    }

    public async Task CreateTransactionAsync(CreateTransactionModel transaction, CancellationToken cancellationToken = default)
    {
        if (transaction is null) throw new ArgumentNullException(nameof(transaction));
        if (transaction.AccountId < 1) throw new ArgumentOutOfRangeException(nameof(transaction.AccountId));
        if (string.IsNullOrEmpty(transaction.Category)) throw new ArgumentException(nameof(transaction.Category));
        
        await using var connection = new SqliteConnection(dbConnectionString);
        connection.Open();
        await using var command = connection.CreateCommand();
        command.CommandText = "insert into transactions (account_id, category, date, info, total) values (@AccountValue, @CategoryValue, @DateValue, @InfoValue, @TotalValue)";

        var account_id = command.CreateParameter();
        account_id.ParameterName = "AccountValue";
        account_id.Value = transaction.AccountId;
        command.Parameters.Add(account_id);

        var category =  command.CreateParameter();
        category.ParameterName = "CategoryValue";
        category.Value = transaction.Category;
        command.Parameters.Add(category);

        var date = command.CreateParameter();
        date.ParameterName = "DateValue";
        date.Value = transaction.Date.ToString("yyyy-MM-dd");
        command.Parameters.Add(date);

        var info = command.CreateParameter();
        info.ParameterName = "InfoValue";
        info.Value = transaction.Info;
        command.Parameters.Add(info);

        var total  = command.CreateParameter();
        total.ParameterName = "TotalValue";
        total.Value = transaction.Total;
        command.Parameters.Add(total);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(dbConnectionString);
        connection.Open();

        return connection;
    }
}
