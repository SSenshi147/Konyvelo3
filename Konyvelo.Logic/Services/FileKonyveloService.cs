using Konyvelo.Logic.Dtos;
using Microsoft.Extensions.Configuration;
using ClosedXML;
using ClosedXML.Excel;

namespace Konyvelo.Logic.Services;

public class FileKonyveloService : IKonyveloService
{
    private const string FILE_PATH_CONFIG_KEY = "DataExcel";

    private readonly IConfiguration _configuration;

    public FileKonyveloService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<List<GetCurrencyDto>> GetAllCurrenciesAsync()
    {
        return Task.FromResult(new List<GetCurrencyDto>());
    }

    public Task<List<GetAccountDto>> GetAllAccountsAsync()
    {
        return Task.FromResult(new List<GetAccountDto>());
    }

    public Task<List<GetTransactionDto>> GetAllTransactionsAsync()
    {
        var path = _configuration[FILE_PATH_CONFIG_KEY];
        if (string.IsNullOrEmpty(path))
            throw new Exception();

        using var workbook = new XLWorkbook(path);
        workbook.TryGetWorksheet("tranzakciók", out var transactionsSheet);
        var list = new List<GetTransactionDto>();
        foreach (var row in transactionsSheet.Rows().Skip(1)) // headers
        {
            var date = DateOnly.FromDateTime(row.Cell(1).Value.GetDateTime());
            var type = row.Cell(2).GetText();
            var name = row.Cell(3).Value.IsText ? row.Cell(3).GetText() : "";
            var total = row.Cell(4).GetDouble();
            var currency = row.Cell(5).GetText();
            var wallet = row.Cell(6).GetText();
            var dto = new GetTransactionDto()
            {
                AccountId = -1,
                AccountName = wallet,
                Category = type,
                CurrencyCode = currency,
                CurrencyId = -1,
                Date = date,
                Id = -1,
                Info = name,
                Total = (decimal)total
            };
            list.Add(dto);
        }

        return Task.FromResult(list);
    }

    public Task CreateCurrencyAsync(CreateCurrencyDto dto)
    {
        return Task.CompletedTask;
    }

    public Task CreateAccountAsync(CreateAccountDto dto)
    {
        return Task.CompletedTask;
    }

    public Task CreateTransactionAsync(CreateTransactionDto dto)
    {
        return Task.CompletedTask;
    }

    public Task UpdateCurrencyAsync(UpdateCurrencyDto dto)
    {
        return Task.CompletedTask;
    }

    public Task UpdateAccountAsync(UpdateAccountDto dto)
    {
        return Task.CompletedTask;
    }

    public Task UpdateTransactionAsync(UpdateTransactionDto dto)
    {
        return Task.CompletedTask;
    }

    public Task DeleteCurrencyAsync(int currencyId)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAccountAsync(int accountId)
    {
        return Task.CompletedTask;
    }

    public Task DeleteTransactionAsync(int transactionId)
    {
        return Task.CompletedTask;
    }

    public async Task<PivotTransactionDto> GetAllPivotTransactionsAsync(DateOnly beginDate, DateOnly endDate)
    {
        var transactions = (await GetAllTransactionsAsync()).Where(x => x.Date >= beginDate && x.Date <= endDate);

        var categories = transactions
            .GroupBy(x => x.Category)
            .Select(x => new PivotTransactionCategory()
            {
                Category = x.Key,
                Transactions = x.GroupBy(y => y.CurrencyCode).Select(y => new PivotTransactionCurrency()
                {
                    CurrencyCode = y.Key,
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

    public Task<DateOnly> GetFirstTransactionDate()
    {
        var path = _configuration[FILE_PATH_CONFIG_KEY];
        if (string.IsNullOrEmpty(path))
            throw new Exception();

        using var workbook = new XLWorkbook(path);
        workbook.TryGetWorksheet("tranzakciók", out var transactionsSheet);
        var dateOnly = DateOnly.FromDateTime(transactionsSheet.Row(1).Cell(1).Value.GetDateTime());

        return Task.FromResult(dateOnly);
    }

    public Task CreateTransferAsync(CreateTransferDto dto)
    {
        return Task.CompletedTask;
    }

    public Task Export()
    {
        return Task.CompletedTask;
    }
}