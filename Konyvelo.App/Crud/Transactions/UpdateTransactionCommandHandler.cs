using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Transactions;

public class UpdateTransactionCommandHandler : UpdateEntityCommandHandler<Transaction, UpdateTransactionCommand>
{
    public UpdateTransactionCommandHandler(ICrudRepo<Transaction> crudRepo) : base(crudRepo)
    {
    }
}
