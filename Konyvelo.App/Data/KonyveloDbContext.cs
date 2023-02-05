using Konyvelo.Logic.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Logic.Data;
public class KonyveloDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public KonyveloDbContext(DbContextOptions<KonyveloDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Currency>().HasKey(x => x.Id);
        modelBuilder.Entity<Wallet>().HasKey(x => x.Id);
        modelBuilder.Entity<Wallet>().HasOne(x => x.Currency).WithMany(x => x.Wallets).HasForeignKey(x => x.CurrencyId);
        modelBuilder.Entity<Transaction>().HasKey(x => x.Id);

        modelBuilder.Entity<Wallet>().Ignore(x => x.TotalCalculated);
    }
}
