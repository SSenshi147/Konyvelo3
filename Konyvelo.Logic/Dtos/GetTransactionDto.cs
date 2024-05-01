namespace Konyvelo.Logic.Dtos;
public class GetCurrencyDto
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public decimal Total { get; init; }
}

public class CreateCurrencyDto
{
    public string Code { get; init; } = string.Empty;
}

public class UpdateCurrencyDto
{
    public required int Id { get; init; }
    public string? Code { get; init; }
}

public class GetAccountDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CurrencyId { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public decimal Total { get; init; }
}

public class CreateAccountDto
{
    public string Name { get; init; } = string.Empty;
    public int CurrencyId { get; init; }
}

public class UpdateAccountDto
{
    public required int Id { get; init; }
    public string? Name { get; init; }
    public int? CurrencyId { get; init; }
}

public class GetTransactionDto
{
    public int Id { get; init; }
    public string Category { get; init; } = string.Empty;
    public string? Info { get; init; }
    public DateOnly Date { get; init; }
    public decimal Total { get; init; }

    public int AccountId { get; init; }
    public string AccountName { get; init; } = string.Empty;
    public int CurrencyId { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
}

public class CreateTransactionDto
{
    public string Category { get; init; } = string.Empty;
    public string? Info { get; init; }
    public DateOnly Date { get; init; }
    public decimal Total { get; init; }
    public int AccountId { get; init; }
}

public class UpdateTransactionDto
{
    public required int Id { get; init; }
    public string? Category { get; init; }
    public string? Info { get; init; }
    public DateOnly? Date { get; init; }
    public decimal? Total { get; init; }
    public int? AccountId { get; init; }
}