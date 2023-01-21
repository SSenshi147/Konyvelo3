using CsharpGoodies.Repo;
using Konyvelo.App.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.App.Repos;
public class CurrencyRepo : CrudRepo<Currency>
{
    public CurrencyRepo(DbContext context) : base(context)
    {
    }
}
