using Konyvelo.Logic.Data;
using Konyvelo.Logic.Domain;
using Konyvelo.Logic.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Services;

public interface IKonyveloService
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

    Task<PivotTransactionDto> GetAllPivotTransactionsAsync(DateOnly beginDate, DateOnly endDate);
    Task<DateOnly> GetFirstTransactionDate();
}

internal class KonyveloService(KonyveloDbContext context) : IKonyveloService
{
    public async Task<List<Transaction>> GetAllTransactions()
    {
        return await context
            .Transactions
            .Include(x => x.Account)
            .ThenInclude(x => x.Currency)
            .ToListAsync();
    }

    public async Task<List<GetCurrencyDto>> GetAllCurrenciesAsync()
    {
        var list = await context.Currencies.Include(x => x.Accounts).ThenInclude(x => x.Transactions).ToListAsync();

        return list.Select(x => new GetCurrencyDto()
        {
            Code = x.Code,
            Id = x.Id,
            Total = x.Total
        }).ToList();
    }

    public async Task<List<GetAccountDto>> GetAllAccountsAsync()
    {
        var list = await context.Accounts.Include(x => x.Currency).Include(x => x.Transactions).ToListAsync();

        return list.Select(x => new GetAccountDto()
        {
            CurrencyCode = x.Currency.Code,
            CurrencyId = x.CurrencyId,
            Id = x.Id,
            Name = x.Name,
            Total = x.Total
        }).ToList();
    }

    public async Task<List<GetTransactionDto>> GetAllTransactionsAsync()
    {
        var list = await context.Transactions.Include(x => x.Account).ThenInclude(x => x.Currency).ToListAsync();

        return list.Select(x => new GetTransactionDto()
        {
            Id = x.Id,
            AccountId = x.AccountId,
            AccountName = x.Account.Name,
            Category = x.Category,
            CurrencyCode = x.Account.Currency.Code,
            CurrencyId = x.Account.Currency.Id,
            Date = x.Date,
            Info = x.Info,
            Total = x.Total
        }).ToList();
    }

    public async Task CreateCurrencyAsync(CreateCurrencyDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrEmpty(dto.Code);

        var model = new Currency()
        {
            Code = dto.Code
        };
        await context.Currencies.AddAsync(model);
        await context.SaveChangesAsync();
    }

    public async Task CreateAccountAsync(CreateAccountDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrEmpty(dto.Name);

        var currency = await context.Currencies.SingleOrDefaultAsync(x => x.Id == dto.CurrencyId) ?? throw new Exception("currency not found");
        var model = new Account()
        {
            Currency = currency,
            CurrencyId = dto.CurrencyId,
            Name = dto.Name
        };
        await context.Accounts.AddAsync(model);
        await context.SaveChangesAsync();
    }

    public async Task CreateTransactionAsync(CreateTransactionDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrEmpty(dto.Category);

        var account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == dto.AccountId) ?? throw new Exception("exception not found");
        var model = new Transaction()
        {
            Account = account,
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
        if (currency is null) return;

        if (!string.IsNullOrEmpty(dto.Code) && currency.Code != dto.Code)
        {
            currency.Code = dto.Code;
        }

        context.Currencies.Update(currency);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(UpdateAccountDto dto)
    {
        var account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == dto.Id);
        if (account is null) return;

        if (!string.IsNullOrEmpty(dto.Name) && account.Name != dto.Name)
        {
            account.Name = dto.Name;
        }

        if (dto.CurrencyId is not null && account.CurrencyId != dto.CurrencyId)
        {
            var currency = await context.Currencies.SingleOrDefaultAsync(x => x.Id == dto.CurrencyId) ?? throw new Exception("currency not found");

            account.Currency = currency;
            account.CurrencyId = dto.CurrencyId.Value;
        }

        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(UpdateTransactionDto dto)
    {
        var transaction = await context.Transactions.SingleOrDefaultAsync(x => x.Id == dto.Id);
        if (transaction is null) return;

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
            var account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == dto.AccountId) ?? throw new Exception("account not found");

            transaction.Account = account;
            transaction.AccountId = dto.AccountId.Value;
        }

        context.Transactions.Update(transaction);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCurrencyAsync(int currencyId)
    {
        var currency = await context.Currencies.SingleOrDefaultAsync(x => x.Id == currencyId);
        if (currency is null) return;

        context.Currencies.Remove(currency);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        var account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == accountId);
        if (account is null) return;

        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int transactionId)
    {
        var transaction = await context.Transactions.SingleOrDefaultAsync(x => x.Id == transactionId);
        if (transaction is null) return;

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();
    }

    public async Task<PivotTransactionDto> GetAllPivotTransactionsAsync(DateOnly beginDate, DateOnly endDate)
    {
        var transactions = await context
            .Transactions
            .Include(x => x.Account)
            .ThenInclude(x => x.Currency)
            .Where(x => x.Date >= beginDate && x.Date <= endDate)
            .ToListAsync();

        var categories = transactions
             .GroupBy(x => x.Category)
             .Select(x => new PivotTransactionCategory()
             {
                 Category = x.Key,
                 Transactions = x.GroupBy(y => y.Account.Currency).Select(y => new PivotTransactionCurrency()
                 {
                     CurrencyCode = y.Key.Code,
                     Transactions = y.Select(z => new PivotTransactionInfo()
                     {
                         Date = z.Date,
                         Info = z.Info ?? "N/A",
                         Total = z.Total
                     }).ToList()
                 }).ToList()
             }).ToList();

        var response = new PivotTransactionDto()
        {
            PivotTransactions = categories
        };

        return response;
    }

    public async Task<DateOnly> GetFirstTransactionDate()
    {
        var query = await context
            .Transactions
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync();

        return query.Date;
    }

    public async Task<PivotTransactionDto> GetPivotTransactions(DateOnly beginDate, DateOnly endDate)
    {
        var transactions = await context
            .Transactions
            .Include(x => x.Account)
            .ThenInclude(x => x.Currency)
            .Where(x => x.Date >= beginDate && x.Date <= endDate)
            .ToListAsync();

        var categories = transactions
             .GroupBy(x => x.Category)
             .Select(x => new PivotTransactionCategory()
             {
                 Category = x.Key,
                 Transactions = x.GroupBy(y => y.Account.Currency).Select(y => new PivotTransactionCurrency()
                 {
                     CurrencyCode = y.Key.Code,
                     Transactions = y.Select(z => new PivotTransactionInfo()
                     {
                         Date = z.Date,
                         Info = z.Info ?? "N/A",
                         Total = z.Total
                     }).ToList()
                 }).ToList()
             }).ToList();

        var response = new PivotTransactionDto()
        {
            PivotTransactions = categories
        };

        return response;
    }

    public async Task<List<Account>> GetAllWallets()
    {
        return await context
            .Accounts
            .Include(x => x.Transactions)
            .Include(x => x.Currency)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<Currency>> GetAllCurrencies()
    {
        return await context.Currencies.ToListAsync();
    }

    public async Task CreateCurrency(Currency currency)
    {
        await context.Currencies.AddAsync(currency);
        await context.SaveChangesAsync();
    }

    public async Task CreateWallet(CreateAccountModel dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));
        if (dto.CurrencyId < 0) throw new ArgumentOutOfRangeException(nameof(dto.CurrencyId));
        if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentException(nameof(dto.Name));

        var currency = await context.Currencies.FirstOrDefaultAsync(x => x.Id == dto.CurrencyId) ?? throw new Exception("currency not found");
        var account = new Account()
        {
            CurrencyId = dto.CurrencyId,
            Currency = currency,
            Name = dto.Name
        };

        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCurrency(Currency currency)
    {
        context.Currencies.Update(currency);
        await context.SaveChangesAsync();
    }

    public async Task UpdateWallet(Account account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCurrency(Currency currency)
    {
        context.Currencies.Remove(currency);
        await context.SaveChangesAsync();
    }

    public async Task DeleteWallet(Account account)
    {
        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }

    public async Task CreateTransaction(CreateTransactionModel dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));
        if (dto.AccountId < 1) throw new ArgumentOutOfRangeException(nameof(dto.AccountId));
        if (string.IsNullOrEmpty(dto.Category)) throw new ArgumentException(nameof(dto.Category));

        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == dto.AccountId) ?? throw new Exception("account not found");
        var transaction = new Transaction()
        {
            Account = account,
            AccountId = dto.AccountId,
            Category = dto.Category,
            Date = dto.Date,
            Info = dto.Info,
            Total = dto.Total
        };

        await context.Transactions.AddAsync(transaction);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTransaction(Transaction transaction)
    {
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTransaction(Transaction transaction)
    {
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();
    }
}

public record CreateTransactionModel(int AccountId, string Category, decimal Total, DateOnly Date, string? Info);
public record CreateAccountModel(int CurrencyId, string Name);