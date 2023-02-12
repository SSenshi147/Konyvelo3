using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Crud.Transactions;

public class GetFirstTransactionDateQueryHandler : IRequestHandler<GetFirstTransactionDateQuery, DateTime>
{
    private readonly ICrudRepo<Transaction> crudRepo;

    public GetFirstTransactionDateQueryHandler(ICrudRepo<Transaction> crudRepo)
    {
        this.crudRepo = crudRepo;
    }

    public async Task<DateTime> Handle(GetFirstTransactionDateQuery request, CancellationToken cancellationToken)
    {
        var query = await crudRepo
            .GetAll()
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync(cancellationToken);

        return query.Date;
    }
}