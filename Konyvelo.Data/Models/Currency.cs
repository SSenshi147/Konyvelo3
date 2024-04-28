using System.ComponentModel.DataAnnotations.Schema;

namespace Konyvelo.Data.Models;

[Table("currencies")]
internal class Currency : Entity
{
    [Column("code")]
    public string Code { get; set; } = string.Empty;
}