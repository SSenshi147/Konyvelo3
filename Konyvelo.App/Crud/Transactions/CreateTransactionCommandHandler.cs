using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Transactions;
public class CreateTransactionCommandHandler : CreateEntityCommandHandler<Transaction, CreateTransactionCommand>
{
    public CreateTransactionCommandHandler(ICrudRepo<Transaction> crudRepo) : base(crudRepo)
    {
    }
}
