using CsharpGoodies.Domain;

namespace Konyvelo.App.Domain;
public class Wallet : Entity
{
    public Currency Currency { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public double Total { get; set; }
    public double TotalCalculated { get; set; }
}
