using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Transactions;

public class GetAllTransactionsQueryHandler : GetAllQueryHandler<Transaction, GetAllTransactionsQuery>
{
    public GetAllTransactionsQueryHandler(ICrudRepo<Transaction> repository) : base(repository)
    {
    }
}