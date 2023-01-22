using CsharpGoodies.Repo;
using Konyvelo.Logic.Data;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Repos;
public class CurrencyRepo : CrudRepo<Currency>
{
    public CurrencyRepo(KonyveloDbContext context) : base(context)
    {
    }
}
