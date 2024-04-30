using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Logic.Domain;

[Table("currencies")]
internal class Currency : Entity
{
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    public List<Account> Accounts { get; set; } = [];

    [NotMapped]
    public decimal Total => Accounts.Sum(x => x.Total);
}