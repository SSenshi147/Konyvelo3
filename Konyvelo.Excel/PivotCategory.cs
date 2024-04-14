namespace Konyvelo.Excel;

public class PivotModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<PivotCategory> Categories { get; set; }

    public PivotModel(DateTime start, DateTime end, IEnumerable<Transaction> transactions)
    {
        Categories = transactions
            .Where(x => x.Date >= start && x.Date <= end)
            .GroupBy(x => x.Category)
            .Select(x => new PivotCategory
            {
                Category = x.Key,
                Currencies = transactions.Where(y => y.Category == x.Key).GroupBy(y => y.Currency).Select(y => new PivotCurrency
                {
                    Currency = y.Key,
                    Transactions = x.Where(z => z.Currency == y.Key).ToList()
                }).ToList()
            })
            .ToList();
        StartDate = start;
        EndDate = end;
    }
}
public class PivotCategory
{
    public string Category { get; set; }
    public List<PivotCurrency> Currencies { get; set; }
}

public class PivotCurrency
{
    public string Currency { get; set; }
    public List<Transaction> Transactions { get; set; }
    public decimal Total => Transactions.Sum(x => x.Total);
}