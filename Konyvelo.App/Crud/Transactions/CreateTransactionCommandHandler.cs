using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Transactions;
public class CreateTransactionCommandHandler : CreateEntityCommandHandler<Transaction, CreateTransactionCommand>
{
    public CreateTransactionCommandHandler(ICrudRepo<Transaction> crudRepo) : base(crudRepo)
    {
    }
}
