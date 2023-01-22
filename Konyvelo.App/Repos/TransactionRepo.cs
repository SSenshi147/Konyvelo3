using CsharpGoodies.Repo;
using Konyvelo.Logic.Data;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Repos;
public class TransactionRepo : CrudRepo<Transaction>
{
    public TransactionRepo(KonyveloDbContext context) : base(context)
    {
    }
}
