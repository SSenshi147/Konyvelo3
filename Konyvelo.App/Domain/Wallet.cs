using CsharpGoodies.Domain;

namespace Konyvelo.Logic.Domain;
public class Wallet : Entity
{
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public double Total { get; set; }
    public double TotalCalculated { get; set; }
}
