using Konyvelo.Data;
using Konyvelo.Domain;
using Konyvelo.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Services;

public class KonyveloService(KonyveloDbContext context)
{
    public async Task<List<Transaction>> GetAllTransactions()
    {
        return await context
            .Transactions
            .Include(x => x.Account)
            .ThenInclude(x => x.Currency)
            .ToListAsync();
    }

    public async Task<DateOnly> GetFirstTransactionDate()
    {
        var query = await context
            .Transactions
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync();

        return query.Date;
    }

    public async Task<PivotTransactionDto> GetPivotTransactions(DateOnly beginDate, DateOnly endDate
        )
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
            .Wallets
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

        var currency = await context.Currencies.FirstOrDefaultAsync(x => x.Id  == dto.CurrencyId) ?? throw new Exception("currency not found");
        var account = new Account()
        {
            CurrencyId = dto.CurrencyId,
            Currency = currency,
            Name = dto.Name
        };

        await context.Wallets.AddAsync(account);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCurrency(Currency currency)
    {
        context.Currencies.Update(currency);
        await context.SaveChangesAsync();
    }

    public async Task UpdateWallet(Account account)
    {
        context.Wallets.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCurrency(Currency currency)
    {
        context.Currencies.Remove(currency);
        await context.SaveChangesAsync();
    }

    public async Task DeleteWallet(Account account)
    {
        context.Wallets.Remove(account);
        await context.SaveChangesAsync();
    }

    public async Task CreateTransaction(CreateTransactionModel dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));
        if (dto.AccountId < 1) throw new ArgumentOutOfRangeException(nameof(dto.AccountId));
        if (string.IsNullOrEmpty(dto.Category)) throw new ArgumentException(nameof(dto.Category));

        var account = await context.Wallets.FirstOrDefaultAsync(x => x.Id == dto.AccountId) ?? throw new Exception("account not found");
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