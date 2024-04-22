using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Konyvelo.Data;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Benchmark;

internal class Program
{
    static async Task Main()
    {
        BenchmarkRunner.Run<SqlTestClass>();
    }
}

[MemoryDiagnoser]
public class SqlTestClass
{
    const string Path = "D:\\personal\\penz\\konyvelo.sqlite";
    const string ConnectionString = $"Data Source={Path}";

    KonyveloCrudService service;

    public SqlTestClass()
    {
        var builder = new DbContextOptionsBuilder<KonyveloDbContext>();
        builder.UseSqlite(ConnectionString);
        var options = builder.Options;
        var context = new KonyveloDbContext(options);
        service = new KonyveloCrudService(context, ConnectionString);
    }

    [Benchmark]
    public async Task TestSubquery()
    {
        await this.service.GetAllAccountsAsync();
    }

    [Benchmark]
    public async Task TestJoin()
    {
        await this.service.GetAllAccountsAsync2();
    }

    [Benchmark]
    public async Task TestInMemory()
    {
        await this.service.GetAllAccountsAsync3();
    }

    [Benchmark]
    public async Task TestLinq()
    {
        await this.service.Get4();
    }
}
