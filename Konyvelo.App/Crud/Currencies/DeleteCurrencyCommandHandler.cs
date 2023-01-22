using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Currencies;

public class DeleteCurrencyCommandHandler : DeleteEntityCommandHandler<Currency, DeleteCurrencyCommand>
{
    public DeleteCurrencyCommandHandler(ICrudRepo<Currency> crudRepo) : base(crudRepo)
    {
    }
}
