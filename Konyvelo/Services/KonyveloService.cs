using CsharpGoodies.Common.Extensions;
using Konyvelo.Data;
using Konyvelo.Domain;
using Konyvelo.Dtos;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Services;

public class KonyveloService(KonyveloDbContext context, IConfiguration configuration)
{
    public async Task<List<Transaction>> GetAllTransactions(CancellationToken cancellationToken = default)
    {
        return await context
            .Transactions
            .Include(x => x.Account)
            .ThenInclude(x => x.Currency)
            .ToListAsync(cancellationToken);
    }

    public async Task<DateOnly> GetFirstTransactionDate(CancellationToken cancellationToken = default)
    {
        var query = await context
            .Transactions
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync(cancellationToken);

        return query.Date;
    }

    public async Task<PivotTransactionDto> GetPivotTransactions(DateOnly begindate, DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        var transactions = await context.Transactions.ToListAsync(cancellationToken);

        var categories = transactions
            .GroupBy(x => x.Category)
            .Select(x => new PivotTransaction
            {
                Category = x.Key,
                Transactions = transactions.Where(y => y.Category == x.Key && y.Date.IsBetween(begindate, endDate))
                    .ToList(),
            })
            .ToList();

        var response = new PivotTransactionDto()
        {
            PivotTransactions = categories
        };

        return response;
    }

    public async Task<List<Account>> GetAllWallets(CancellationToken cancellationToken = default)
    {
        return await context
            .Wallets
            .Include(x => x.Transactions)
            .Include(x => x.Currency)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Currency>> GetAllCurrencies(CancellationToken cancellationToken = default)
    {
        return await context.Currencies.ToListAsync(cancellationToken);
    }

    public async Task CreateCurrency(Currency currency, CancellationToken cancellationToken = default)
    {
        await context.Currencies.AddAsync(currency, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateWallet(Account account, CancellationToken cancellationToken = default)
    {
        await context.Wallets.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCurrency(Currency currency, CancellationToken cancellationToken = default)
    {
        context.Currencies.Update(currency);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateWallet(Account account, CancellationToken cancellationToken = default)
    {
        context.Wallets.Update(account);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCurrency(Currency currency, CancellationToken cancellationToken = default)
    {
        context.Currencies.Remove(currency);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteWallet(Account account, CancellationToken cancellationToken = default)
    {
        context.Wallets.Remove(account);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateTransaction(CreateTransactionModel transaction, CancellationToken cancellationToken = default)
    {
        if (transaction is null) throw new ArgumentNullException(nameof(transaction));
        if (transaction.AccountId < 1) throw new ArgumentOutOfRangeException(nameof(transaction.AccountId));
        if (transaction.Category.IsNullOrEmpty()) throw new ArgumentException(nameof(transaction.Category));
        
        await using var connection = new SqliteConnection(configuration["SqliteConnectionString"]);
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

    public async Task UpdateTransaction(Transaction transaction, CancellationToken cancellationToken = default)
    {
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTransaction(Transaction transaction, CancellationToken cancellationToken = default)
    {
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync(cancellationToken);
    }

    // public async Task Import()
    // {
    //var path = this.configuration["ImportCsv"] ?? throw new Exception("asd");

    //var lines = await File.ReadAllLinesAsync(path, cancellationToken);

    //foreach (var line in lines)
    //{
    //    var split = line.Split(';');

    //    var transaction = new Transaction
    //    {
    //        Date = DateTime.Parse(split[0]),
    //        Category = split[1],
    //        Name = split[2],
    //        Type = (TransactionType)int.Parse(split[3]),
    //        Total = decimal.Parse(split[4]),
    //        WalletId = new Guid("367E547A-448E-42C2-BDCB-8ED6FAA55B1F"),
    //    };

    //    await crudRepo.AddAsync(transaction, cancellationToken);
    //}

    //await crudRepo.SaveChangesAsync(cancellationToken);
    // }
}

public record CreateTransactionModel(int AccountId, string Category, decimal Total, DateOnly Date, string? Info);