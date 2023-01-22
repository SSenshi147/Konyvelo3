using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Repos;
public class WalletRepo : CrudRepo<Wallet>
{
    public WalletRepo(DbContext context) : base(context)
    {
    }
}
