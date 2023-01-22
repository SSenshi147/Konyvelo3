using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Currencies;

public class GetAllCurrenciesQueryHandler : GetAllQueryHandler<Currency, GetAllCurrenciesQuery>
{
    public GetAllCurrenciesQueryHandler(ICrudRepo<Currency> repository) : base(repository)
    {
    }
}