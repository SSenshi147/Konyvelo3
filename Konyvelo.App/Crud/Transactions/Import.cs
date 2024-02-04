using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvelo.Logic.Crud.Transactions;

public class Import : IRequestHandler<ImportRequest>
{
    private readonly ICrudRepo<Transaction> crudRepo;
    private readonly IConfiguration configuration;

    public Import(ICrudRepo<Transaction> crudRepo, IConfiguration configuration)
    {
        this.crudRepo = crudRepo;
        this.configuration = configuration;
    }

    public async Task Handle(ImportRequest request, CancellationToken cancellationToken)
    {
        //var path = this.configuration["ImportCsv"] ?? throw new Exception("asd");

        //var lines = await File.ReadAllLinesAsync(path, cancellationToken);

        //foreach (var line in lines)
        //{
        //    var split = line.Split(';');

        //    var transaction = new Transaction
        //    {
        //        Date = DateTime.Parse(split[0]),
        //        Category = split[1],
        //        Name = split[2],
        //        Type = (TransactionType)int.Parse(split[3]),
        //        Total = decimal.Parse(split[4]),
        //        WalletId = new Guid("367E547A-448E-42C2-BDCB-8ED6FAA55B1F"),
        //    };

        //    await crudRepo.AddAsync(transaction, cancellationToken);
        //}

        //await crudRepo.SaveChangesAsync(cancellationToken);
    }
}

public class ImportRequest : IRequest
{
}
