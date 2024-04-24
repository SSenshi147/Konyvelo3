using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dapper;
using Konyvelo.Data;
using Microsoft.EntityFrameworkCore;

namespace Konyvelo.Benchmark;

internal class Program
{
    static async Task Main()
    {
        //var asd = new SqlTestClass();
        //await asd.TestEf();

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
        SqlMapper.AddTypeHandler(new DapperSqliteDateOnlyTypeHandler());
        var builder = new DbContextOptionsBuilder<KonyveloDbContext>();
        builder.UseSqlite(ConnectionString);
        var options = builder.Options;
        var context = new KonyveloDbContext(options);
        service = new KonyveloCrudService(context, ConnectionString);
    }

    [Benchmark]
    public async Task TestEf()
    {
        await this.service.GetAllTransactionsAsync();
    }

    [Benchmark]
    public async Task TestSql()
    {
        await this.service.GetAllTransactionsAsync2();
    }
}
