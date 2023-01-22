using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Transactions;

public class DeleteTransactionCommandHandler : DeleteEntityCommandHandler<Transaction, DeleteTransactionCommand>
{
    public DeleteTransactionCommandHandler(ICrudRepo<Transaction> crudRepo) : base(crudRepo)
    {
    }
}
