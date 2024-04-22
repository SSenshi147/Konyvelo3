namespace Konyvelo.Data.Dtos;

public class PivotTransactionDto
{
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Category { get; set; }
    public List<GetTransactionDto> Transactions { get; set; } = [];
}