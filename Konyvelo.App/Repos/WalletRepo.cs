using CsharpGoodies.Repo;
using Konyvelo.App.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.App.Repos;
public class WalletRepo : CrudRepo<Wallet>
{
    public WalletRepo(DbContext context) : base(context)
    {
    }
}
