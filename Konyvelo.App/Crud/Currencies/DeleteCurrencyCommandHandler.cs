using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Currencies;

public class DeleteCurrencyCommandHandler : DeleteEntityCommandHandler<Currency, DeleteCurrencyCommand>
{
    public DeleteCurrencyCommandHandler(ICrudRepo<Currency> crudRepo) : base(crudRepo)
    {
    }
}
