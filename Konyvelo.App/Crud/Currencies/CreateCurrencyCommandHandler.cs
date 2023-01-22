using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Currencies;
public class CreateCurrencyCommandHandler : CreateEntityCommandHandler<Currency, CreateCurrencyCommand>
{
    public CreateCurrencyCommandHandler(ICrudRepo<Currency> crudRepo) : base(crudRepo)
    {
    }
}
