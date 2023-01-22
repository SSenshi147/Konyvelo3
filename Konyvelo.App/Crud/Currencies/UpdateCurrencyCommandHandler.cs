using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Currencies;

public class UpdateCurrencyCommandHandler : UpdateEntityCommandHandler<Currency, UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandHandler(ICrudRepo<Currency> crudRepo) : base(crudRepo)
    {
    }
}
