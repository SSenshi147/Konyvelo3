using CsharpGoodies.Common.Extensions;
using Konyvelo.Data;
using Konyvelo.Domain;
using Konyvelo.Dtos;
using Konyvelo.Pages;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Services;

public class KonyveloService(KonyveloDbContext context, IConfiguration configuration)
{
    public async Task<List<Transaction>> GetAllTransactions(CancellationToken cancellationToken = default)
    {
        return await context
            .Transactions
            .Where(x => !x.IsDeleted)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .ToListAsync(cancellationToken);
    }

    public async Task<DateTime> GetFirstTransactionDate(CancellationToken cancellationToken = default)
    {
        var query = await context
            .Transactions
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync(cancellationToken);

        return query.Date;
    }

    public async Task<PivotTransactionDto> GetPivotTransactions(DateTime begindate, DateTime endDate,
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

    public async Task<List<Wallet>> GetAllWallets(CancellationToken cancellationToken = default)
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
        currency.CreateNow();
        await context.Currencies.AddAsync(currency, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateWallet(Wallet wallet, CancellationToken cancellationToken = default)
    {
        wallet.CreateNow();
        await context.Wallets.AddAsync(wallet, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCurrency(Currency currency, CancellationToken cancellationToken = default)
    {
        currency.UpdateNow();
        context.Currencies.Update(currency);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateWallet(Wallet wallet, CancellationToken cancellationToken = default)
    {
        wallet.UpdateNow();
        context.Wallets.Update(wallet);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCurrency(Currency currency, CancellationToken cancellationToken = default)
    {
        currency.Delete();
        context.Currencies.Update(currency);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteWallet(Wallet wallet, CancellationToken cancellationToken = default)
    {
        wallet.Delete();
        context.Wallets.Update(wallet);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateTransaction(Transaction transaction, CancellationToken cancellationToken = default)
    {
        transaction.CreateNow();
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateTransaction(Transaction transaction, CancellationToken cancellationToken = default)
    {
        transaction.UpdateNow();
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteTransaction(Transaction transaction, CancellationToken cancellationToken = default)
    {
        transaction.Delete();
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Export()
    {
        var path = configuration["ExportCsv"] ?? throw new Exception("export not found");
        var alltrans = await context.Transactions.ToListAsync();
        var sw = new StreamWriter(path);
        foreach (var transaction in alltrans)
        {
            var sum = transaction.Type == TransactionType.Expense
                ? -Math.Abs(transaction.Total)
                : Math.Abs(transaction.Total); 
            await sw.WriteLineAsync($"{transaction.Date:yyyy-MM-dd};{transaction.Category};{transaction.Name};{sum};{transaction.Wallet.Currency.Code};{transaction.Wallet.Name}");
        }
        sw.Close();
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