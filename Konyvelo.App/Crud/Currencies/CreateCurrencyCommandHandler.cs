using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Currencies;
public class CreateCurrencyCommandHandler : CreateEntityCommandHandler<Currency, CreateCurrencyCommand>
{
    public CreateCurrencyCommandHandler(ICrudRepo<Currency> crudRepo) : base(crudRepo)
    {
    }
}
