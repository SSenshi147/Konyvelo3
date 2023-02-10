using Konyvelo.Logic.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvelo.Logic.Crud.Transactions;
public class GetPivotTransactionsQuery : IRequest<PivotTransactionDto>
{
    public DateTime BeginDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime EndDate { get; set; } = DateTime.Today;
}

public class PivotTransactionDto
{
    public List<PivotTransaction> PivotTransactions { get; set; } = new();
}

public class PivotTransaction
{
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Category { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
}
