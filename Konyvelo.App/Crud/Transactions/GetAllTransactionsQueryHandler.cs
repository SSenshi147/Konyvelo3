using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Transactions;

public class GetAllTransactionsQueryHandler : GetAllQueryHandler<Transaction, GetAllTransactionsQuery>
{
    public GetAllTransactionsQueryHandler(ICrudRepo<Transaction> repository) : base(repository)
    {
    }
}