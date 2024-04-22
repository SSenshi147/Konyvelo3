namespace Konyvelo.Data.Dtos;

public class PivotTransactionDto
{
    public required string Category { get; init; }
    public List<GetTransactionDto> Transactions { get; init; } = [];
}