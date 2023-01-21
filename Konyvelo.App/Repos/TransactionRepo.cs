using CsharpGoodies.Repo;
using Konyvelo.App.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.App.Repos;
public class TransactionRepo : CrudRepo<Transaction>
{
    public TransactionRepo(DbContext context) : base(context)
    {
    }
}
