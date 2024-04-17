using Konyvelo.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Data;
public class KonyveloDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Account> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public KonyveloDbContext(DbContextOptions<KonyveloDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>().HasOne(x => x.Currency).WithMany(x => x.Accounts).HasForeignKey(x => x.CurrencyId);
        modelBuilder.Entity<Transaction>().HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountId);
    }
}
