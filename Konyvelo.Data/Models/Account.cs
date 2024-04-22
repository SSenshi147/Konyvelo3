using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Data.Models;

[Table("accounts")]
internal class Account : Entity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("currency_id")]
    public int CurrencyId { get; set; }
}