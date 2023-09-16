using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Crud.Wallets;

public class GetAllWalletsQueryHandler : GetAllQueryHandler<Wallet, GetAllWalletsQuery>
{
    public GetAllWalletsQueryHandler(ICrudRepo<Wallet> repository) : base(repository)
    {
    }

    public override async Task<List<Wallet>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        return await _crudRepo
            .GetAll()
            .Include(x => x.Transactions)
            .Include(x => x.Currency)
            .Where(WhereExpression)
            .OrderBy(x => x.Name)
            .Select(SelectExpression)
            .ToListAsync(cancellationToken);
    }
}
