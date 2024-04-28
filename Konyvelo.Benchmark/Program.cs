using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dapper;
using Konyvelo.Data;
using Konyvelo.Data.Dtos;
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
    const string Path = "D:\\personal\\penz\\konyvelo_test.sqlite";
    const string ConnectionString = $"Data Source={Path}";

    private KonyveloDbContext context;
    KonyveloCrudService service;

    public SqlTestClass()
    {
        SqlMapper.AddTypeHandler(new DapperSqliteDateOnlyTypeHandler());
        var builder = new DbContextOptionsBuilder<KonyveloDbContext>();
        builder.UseSqlite(ConnectionString);
        var options = builder.Options;
        context = new KonyveloDbContext(options);
        service = new KonyveloCrudService(context, ConnectionString);
    }

    [Benchmark]
    public async Task TestEf()
    {
        await this.service.CreateCurrencyAsync(new CreateCurrencyDto()
        {
            Code = "HUF"
        });
        await context.Currencies.ExecuteDeleteAsync();
    }

    [Benchmark]
    public async Task TestSql()
    {
        await this.service.CreateCurrencyAsync2(new CreateCurrencyDto()
        {
            Code = "HUF"
        });
        await context.Currencies.ExecuteDeleteAsync();
    }
}
