using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Crud.Transactions;

public class GetAllTransactionsQueryHandler : GetAllQueryHandler<Transaction, GetAllTransactionsQuery>
{
    public GetAllTransactionsQueryHandler(ICrudRepo<Transaction> repository) : base(repository)
    {
    }

    public override async Task<List<Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _crudRepo
            .GetAll()
            .Include(x => x.Wallet)
                .ThenInclude(x => x.Currency)
            .Where(WhereExpression)
            .Select(SelectExpression)
            .ToListAsync(cancellationToken);
    }
}