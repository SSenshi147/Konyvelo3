namespace Konyvelo.Logic.Dtos;

public class CreateTransferDto
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Total { get; set; } = 1;
    public string Category { get; set; } = "Transfer";
    public string? Info { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}