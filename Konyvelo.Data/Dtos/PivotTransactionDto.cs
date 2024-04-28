namespace Konyvelo.Data.Dtos;

public class PivotTransactionDto
{
    public required string Category { get; init; }
    public List<GetTransactionDto> Transactions { get; init; } = [];
}

public class PivotBaseDto
{
    public int CurrencyId { get; set; }
    public int AccountId { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
}