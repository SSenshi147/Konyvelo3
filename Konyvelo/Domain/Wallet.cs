using CsharpGoodies.Common.Domain;

namespace Konyvelo.Domain;

public class Wallet : Entity
{
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = default!;

    public string Name { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public decimal TotalCalculated
    {
        get
        {
            var expenses = Transactions.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Total);
            var incomes = Transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Total);
            var total = incomes - expenses;
            return total;
        }
    }

    public List<Transaction> Transactions { get; set; } = new();
}