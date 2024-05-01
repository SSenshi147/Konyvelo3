using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Logic.Domain;

[Table("transactions")]
internal class Transaction : Entity
{
    [Column("category")]
    public string Category { get; set; } = string.Empty;
    [Column("info")]
    public string? Info { get; set; }
    [Column("date")]
    public DateOnly Date { get; set; }
    [Column("total")]
    public decimal Total { get; set; }

    public Account Account { get; set; } = new();
    [Column("account_id")]
    public int AccountId { get; set; }
}