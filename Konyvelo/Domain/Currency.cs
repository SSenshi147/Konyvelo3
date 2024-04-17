using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Domain;

[Table("currencies")]
public class Currency : Entity
{
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    public List<Account> Accounts { get; set; } = [];

    public decimal Total => Accounts.Sum(x => x.Total);
}