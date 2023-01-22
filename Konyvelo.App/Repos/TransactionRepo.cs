using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Repos;
public class TransactionRepo : CrudRepo<Transaction>
{
    public TransactionRepo(DbContext context) : base(context)
    {
    }
}
