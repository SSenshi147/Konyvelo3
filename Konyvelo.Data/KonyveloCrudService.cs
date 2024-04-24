using CsharpGoodies.Common.Extensions;
using Dapper;
using Konyvelo.Data.Dtos;
using Konyvelo.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Konyvelo.Data;

public interface IKonyveloCrudService
{
    Task<List<GetCurrencyDto>> GetAllCurrenciesAsync();
    Task<List<GetAccountDto>> GetAllAccountsAsync();
    Task<List<GetTransactionDto>> GetAllTransactionsAsync();

    Task CreateCurrencyAsync(CreateCurrencyDto dto);
    Task CreateAccountAsync(CreateAccountDto dto);
    Task CreateTransactionAsync(CreateTransactionDto dto);

    Task UpdateCurrencyAsync(UpdateCurrencyDto dto);
    Task UpdateAccountAsync(UpdateAccountDto dto);
    Task UpdateTransactionAsync(UpdateTransactionDto dto);

    Task DeleteCurrencyAsync(int currencyId);
    Task DeleteAccountAsync(int accountId);
    Task DeleteTransactionAsync(int transactionId);

    Task<GetPivotTransactionsDto> GetAllPivotTransactionsAsync(DateOnly beginDate, DateOnly endDate);
    Task<DateOnly> GetFirstTransactionDate();
}

internal class KonyveloCrudService : IKonyveloCrudService
{
    private const string CONNECTION_STRING_KEY = "SqliteConnectionString";
    private const string SqlFolderPath = @"D:\repos\Konyvelo\Konyvelo.Data\Sqls\";
    // TODO: ez itt nagyon nem jó

    private readonly KonyveloDbContext context;
    private readonly string connectionString;
    private readonly ConcurrentDictionary<string, string> _queries = [];

    public KonyveloCrudService(KonyveloDbContext context, IConfiguration config)
    {
        this.context = context;
        connectionString = config[CONNECTION_STRING_KEY] ?? throw new Exception();
    }

    internal KonyveloCrudService(KonyveloDbContext context, string connectionString)
    {
        this.context = context;
        this.connectionString = connectionString;
    }

    public async Task<List<GetCurrencyDto>> GetAllCurrenciesAsync()
    {
        var sql = await GetQueryString("get_all_currencies");

        await using var conn = new SqliteConnection(connectionString);
        var query = await conn.QueryAsync<GetCurrencyDto>(sql);

        return query.ToList();
    }

    public async Task<List<GetAccountDto>> GetAllAccountsAsync()
    {
        var sql = await GetQueryString("get_all_accounts");

        await using var conn = new SqliteConnection(connectionString);
        var query = await conn.QueryAsync<GetAccountDto>(sql);

        return query.ToList();
    }

    public async Task<List<GetTransactionDto>> GetAllTransactionsAsync()
    {
        var query = from transaction in context.Transactions
                    join account in context.Accounts on transaction.AccountId equals account.Id
                    join currency in context.Currencies on account.CurrencyId equals currency.Id
                    select new GetTransactionDto()
                    {
                        AccountId = account.Id,
                        CurrencyId = currency.Id,
                        AccountName = account.Name,
                        Category = transaction.Category,
                        CurrencyCode = currency.Code,
                        Date = transaction.Date,
                        Id = transaction.Id,
                        Info = transaction.Info,
                        Total = transaction.Total
                    };
        
        return await query.ToListAsync();
    }

    public async Task<List<GetTransactionDto>> GetAllTransactionsAsync2()
    {
        var sql = await GetQueryString("get_all_transactions");

        await using var conn = new SqliteConnection(connectionString);
        var query = await conn.QueryAsync<GetTransactionDto>(sql);

        return query.ToList();
    }

    public async Task CreateCurrencyAsync(CreateCurrencyDto dto)
    {
        var model = new Currency()
        {
            Code = dto.Code
        };
        await context.Currencies.AddAsync(model);
        await context.SaveChangesAsync();
    }

    public async Task CreateAccountAsync(CreateAccountDto dto)
    {
        var model = new Account()
        {
            CurrencyId = dto.CurrencyId,
            Name = dto.Name
        };
        await context.Accounts.AddAsync(model);
        await context.SaveChangesAsync();
    }

    public async Task CreateTransactionAsync(CreateTransactionDto dto)
    {
        var model = new Transaction()
        {
            AccountId = dto.AccountId,
            Category = dto.Category,
            Date = dto.Date,
            Info = dto.Info,
            Total = dto.Total
        };
        await context.Transactions.AddAsync(model);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCurrencyAsync(UpdateCurrencyDto dto)
    {
        var currency = await context.Currencies.SingleOrDefaultAsync(x => x.Id == dto.Id);
        if (currency is null)
        {
            return;
        }

        if (currency.Code != dto.Code)
        {
            currency.Code = dto.Code;
        }

        context.Currencies.Update(currency);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(UpdateAccountDto dto)
    {
        var account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == dto.Id);
        if (account is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(dto.Name) && account.Name != dto.Name)
        {
            account.Name = dto.Name;
        }

        if (dto.CurrencyId is not null && account.CurrencyId != dto.CurrencyId)
        {
            account.CurrencyId = dto.CurrencyId.Value;
        }

        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(UpdateTransactionDto dto)
    {
        var transaction = await context.Transactions.SingleOrDefaultAsync(x => x.Id == dto.Id);
        if (transaction is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(dto.Category) && transaction.Category != dto.Category)
        {
            transaction.Category = dto.Category;
        }

        if (transaction.Info != dto.Info)
        {
            transaction.Info = dto.Info;
        }

        if (dto.Date is not null && transaction.Date != dto.Date)
        {
            transaction.Date = dto.Date.Value;
        }

        if (dto.Total is not null && transaction.Total != dto.Total)
        {
            transaction.Total = dto.Total.Value;
        }

        if (dto.AccountId is not null && transaction.AccountId != dto.AccountId)
        {
            transaction.AccountId = dto.AccountId.Value;
        }

        context.Transactions.Update(transaction);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCurrencyAsync(int currencyId)
    {
        var currency = await context.Currencies.SingleOrDefaultAsync(x => x.Id == currencyId);
        if (currency is null)
        {
            return;
        }

        context.Currencies.Remove(currency);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        var account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == accountId);
        if (account is null)
        {
            return;
        }

        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int transactionId)
    {
        var transaction = await context.Transactions.SingleOrDefaultAsync(x => x.Id == transactionId);
        if (transaction is null)
        {
            return;
        }

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();
    }

    public async Task<DateOnly> GetFirstTransactionDate()
    {
        var query = await context
            .Transactions
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync();

        return query.Date;
    }

    public async Task<GetPivotTransactionsDto> GetAllPivotTransactionsAsync(DateOnly beginDate, DateOnly endDate)
    {
        var transactions = await GetAllTransactionsAsync();

        var categories = transactions
            .GroupBy(x => x.Category)
            .Select(x => new PivotTransactionDto()
            {
                Category = x.Key,
                Transactions = transactions.Where(y => y.Category == x.Key && y.Date.IsBetween(beginDate, endDate))
                    .Select(x => new GetTransactionDto()
                    {
                        AccountId = x.AccountId,
                        AccountName = x.AccountName,
                        Category = x.Category,
                        CurrencyCode = x.CurrencyCode,
                        CurrencyId = x.CurrencyId,
                        Date = x.Date,
                        Id = x.Id,
                        Info = x.Info,
                        Total = x.Total
                    })
                    .ToList(),
            })
            .ToList();

        var response = new GetPivotTransactionsDto()
        {
            PivotTransactions = categories
        };

        return response;
    }

    private async Task<string> GetQueryString(string key)
    {
        return _queries.GetOrAdd(key, await File.ReadAllTextAsync($"{SqlFolderPath}{key}.sql"));
    }
}
