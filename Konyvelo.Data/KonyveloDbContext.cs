using Konyvelo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Data;
internal class KonyveloDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public KonyveloDbContext(DbContextOptions<KonyveloDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>().HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId);
        modelBuilder.Entity<Transaction>().HasOne<Account>().WithMany().HasForeignKey(x => x.AccountId);
    }
}
