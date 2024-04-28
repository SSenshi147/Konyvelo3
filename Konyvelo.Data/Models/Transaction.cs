using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Data.Models;

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
    public double Total { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }
}