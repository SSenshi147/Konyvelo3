﻿using CsharpGoodies.Common.Extensions;
using Dapper;
using Konyvelo.Data.Dtos;
using Konyvelo.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    Task<GetPivotTransactionsDto> GetAllPivotTransactionsAsync(DateOnly begindate, DateOnly endDate);
    Task<DateOnly> GetFirstTransactionDate();
}

internal class KonyveloCrudService(KonyveloDbContext context, IConfiguration config) : IKonyveloCrudService
{
    private const string CONNECTION_STRING_KEY = "SqliteConnectionString";

    public async Task<List<GetCurrencyDto>> GetAllCurrenciesAsync()
    {
        return await context.Currencies.Select(x => new GetCurrencyDto()
        {
            Code = x.Code,
            Id = x.Id
        }).ToListAsync();
    }

    public async Task<List<GetAccountDto>> GetAllAccountsAsync()
    {
        const string sql = "select c.code CurrencyCode, AccountName Name, CurrencyId, AccountId Id, AccountTotal Total from currencies c join (select a.name AccountName, a.currency_id CurrencyId, AccountId, AccountTotal from accounts a join (select t.account_id AccountId, sum(t.total) AccountTotal FROM transactions t group by AccountId) on a.id = AccountId) on c.id = CurrencyId";

        await using var conn = new SqliteConnection(config[CONNECTION_STRING_KEY]);
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

        if (account.Name != dto.Name)
        {
            account.Name = dto.Name;
        }

        if (account.CurrencyId != dto.CurrencyId)
        {
            account.CurrencyId = dto.CurrencyId;
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

        if (transaction.Category != dto.Category)
        {
            transaction.Category = dto.Category;
        }

        if (transaction.Info != dto.Info)
        {
            transaction.Info = dto.Info;
        }

        if (transaction.Date != dto.Date)
        {
            transaction.Date = dto.Date;
        }

        if (transaction.Total != dto.Total)
        {
            transaction.Total = dto.Total;
        }

        if (transaction.AccountId != dto.AccountId)
        {
            transaction.AccountId = dto.AccountId;
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

    public async Task<GetPivotTransactionsDto> GetAllPivotTransactionsAsync(DateOnly begindate, DateOnly endDate)
    {
        var transactions = await context.Transactions.ToListAsync();

        var categories = transactions
            .GroupBy(x => x.Category)
            .Select(x => new PivotTransactionDto()
            {
                Category = x.Key,
                Transactions = transactions.Where(y => y.Category == x.Key && y.Date.IsBetween(begindate, endDate))
                    .Select(x => new GetTransactionDto()
                    {
                        AccountId = x.AccountId,
                        AccountName = "", // TODO
                        Category = x.Category,
                        CurrencyCode = "",
                        CurrencyId = 0,
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
}
