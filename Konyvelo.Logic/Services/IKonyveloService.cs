using Konyvelo.Logic.Dtos;

namespace Konyvelo.Logic.Services;

public interface IKonyveloService
{
    Task<List<GetCurrencyDto>> GetAllCurrenciesAsync();
    Task<List<GetAccountDto>> GetAllAccountsAsync();
    Task<List<GetTransactionDto>> GetAllTransactionsAsync();

    Task CreateCurrencyAsync(CreateCurrencyDto dto);
    Task CreateAccountAsync(CreateAccountDto dto);
    Task CreateTransactionAsync(CreateTransactionDto dto);

    Task UpdateCurrencyAsync(UpdateCurrencyDto dto);
    Task UpdateAccountAsync(UpdateAccountDto dto);
    Task UpdateTransactionAsync(UpdateTransactionDto dto);

    Task DeleteCurrencyAsync(int currencyId);
    Task DeleteAccountAsync(int accountId);
    Task DeleteTransactionAsync(int transactionId);

    Task<PivotTransactionDto> GetAllPivotTransactionsAsync(DateOnly beginDate, DateOnly endDate);
    Task<DateOnly> GetFirstTransactionDate();
    Task CreateTransferAsync(CreateTransferDto dto);
    Task Export();
}
