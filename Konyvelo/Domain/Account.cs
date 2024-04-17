using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Domain;

[Table("accounts")]
public class Account : Entity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public Currency Currency { get; set; } = new();
    [Column("currency_id")]
    public int CurrencyId { get; set; }

    public List<Transaction> Transactions { get; set; } = [];

    [NotMapped]
    public decimal Total => Transactions.Sum(x => x.Total);
}