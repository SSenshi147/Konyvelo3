using CsharpGoodies.Repo;
using Konyvelo.Logic.Data;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Repos;
public class WalletRepo : CrudRepo<Wallet>
{
    public WalletRepo(KonyveloDbContext context) : base(context)
    {
    }
}
