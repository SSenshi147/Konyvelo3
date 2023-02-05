using CsharpGoodies.Domain;

namespace Konyvelo.Logic.Domain;
public class Transaction : Entity
{
    public DateTime Date { get; set; }
    
    public string Category { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public TransactionType Type { get; set; }
    
    public double Value { get; set; }
    public int Count { get; set; }
    public double TotalValue => Value * Count;

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = default!;
}
