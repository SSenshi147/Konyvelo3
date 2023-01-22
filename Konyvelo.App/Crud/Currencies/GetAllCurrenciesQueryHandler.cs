using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Currencies;

public class GetAllCurrenciesQueryHandler : GetAllQueryHandler<Currency, GetAllCurrenciesQuery>
{
    public GetAllCurrenciesQueryHandler(ICrudRepo<Currency> repository) : base(repository)
    {
    }
}