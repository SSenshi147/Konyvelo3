namespace Konyvelo.Logic.Dtos;

public class PivotTransactionDto
{
    public List<PivotTransactionCategory> PivotTransactions { get; set; } = [];
    public List<PivotTransactionCategory> DisplayTransactions => PivotTransactions
        .Where(x => x.ShouldDisplay)
        .OrderBy(x => x.Category)
        .ToList();
    public List<GrandTotalDto> GrandTotals => PivotTransactions
        .Where(x => x.ShouldDisplay)
        .SelectMany(x => x.Transactions)
        .GroupBy(x => x.CurrencyCode)
        .Select(x => new GrandTotalDto()
        {
            CurrencyCode = x.Key,
            GrandTotal = x.Sum(y => y.CurrencyTotal)
        }).ToList();
}

public class PivotTransactionCategory
{
    public string Category { get; set; }
    public List<PivotTransactionCurrency> Transactions { get; set; } = [];
    public List<PivotTransactionCurrency> DisplayTransactions => Transactions.Where(x => x.ShouldDisplay).ToList();
    public bool ShouldDisplay => DisplayTransactions.Count > 0;
}

public class PivotTransactionCurrency
{
    public string CurrencyCode { get; set; } = string.Empty;
    public List<PivotTransactionInfo> Transactions { get; set; } = [];
    public decimal CurrencyTotal => Transactions.Sum(x => x.Total);
    public bool ShouldDisplay => CurrencyTotal != 0;
}

public class PivotTransactionInfo
{
    public decimal Total { get; set; }
    public string Info { get; set; } = "N/A";
    public DateOnly Date { get; set; }
}

public class GrandTotalDto
{
    public string CurrencyCode { get; set; }
    public decimal GrandTotal { get; set; }
}