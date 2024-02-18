using Konyvelo.Logic.Domain;

namespace Konyvelo.Dtos;

public class PivotTransaction
{
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Category { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
}