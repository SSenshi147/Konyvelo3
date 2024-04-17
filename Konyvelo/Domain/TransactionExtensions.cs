namespace Konyvelo.Domain;

public static class TransactionExtensions
{
    public static decimal GetTotal(this IEnumerable<Transaction> transactions)
    {
        return transactions.Sum(x => x.Total);
    }
}