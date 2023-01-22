using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Transactions;

public class UpdateTransactionCommandHandler : UpdateEntityCommandHandler<Transaction, UpdateTransactionCommand>
{
    public UpdateTransactionCommandHandler(ICrudRepo<Transaction> crudRepo) : base(crudRepo)
    {
    }
}
