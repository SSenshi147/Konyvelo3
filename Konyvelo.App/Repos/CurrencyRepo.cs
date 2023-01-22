using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Repos;
public class CurrencyRepo : CrudRepo<Currency>
{
    public CurrencyRepo(DbContext context) : base(context)
    {
    }
}
