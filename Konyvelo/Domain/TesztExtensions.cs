using Konyvelo.Logic.Domain;

namespace Konyvelo.Domain;

public static class TesztExtensions
{
    public static List<Transaction> GetExpenses(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Expense).ToList();
    }

    public static decimal GetExpensesTotal(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Expense).Sum(x => x.Total);
    }

    public static List<Transaction> GetIncomes(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Income).ToList();
    }

    public static decimal GetIncomesTotal(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Income).Sum(x => x.Total);
    }

    public static decimal GetTotal(this IEnumerable<Transaction> transactions)
    {
        var expenses = transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Expense).Sum(x => x.Total);
        var incomes = transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Income).Sum(x => x.Total);
        var total = incomes - expenses;

        return total;
    }
}