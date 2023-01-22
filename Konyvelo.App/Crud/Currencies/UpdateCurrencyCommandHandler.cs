using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Currencies;

public class UpdateCurrencyCommandHandler : UpdateEntityCommandHandler<Currency, UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandHandler(ICrudRepo<Currency> crudRepo) : base(crudRepo)
    {
    }
}
