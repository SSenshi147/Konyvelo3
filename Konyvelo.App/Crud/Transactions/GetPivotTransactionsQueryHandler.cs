using CsharpGoodies.Extensions;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konyvelo.Logic.Crud.Transactions;
public class GetPivotTransactionsQueryHandler : IRequestHandler<GetPivotTransactionsQuery, PivotTransactionDto>
{
    private readonly ICrudRepo<Transaction> crudRepo;

    public GetPivotTransactionsQueryHandler(ICrudRepo<Transaction> crudRepo)
    {
        this.crudRepo = crudRepo;
    }

    public Task<PivotTransactionDto> Handle(GetPivotTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = crudRepo.GetAll().ToList();

        var categories = transactions
            .GroupBy(x => x.Category)
            .Select(x => new PivotTransaction()
            {
                Category = x.Key,
                Transactions = transactions.Where(y => y.Category == x.Key && y.Date.IsBetween(request.BeginDate, request.EndDate)).ToList(),
            })
            .ToList();

        var response = new PivotTransactionDto()
        {
            PivotTransactions = categories
        };

        return response.AsTaskResult();
    }
}

public static class TesztExtensions
{
    public static List<Transaction> GetExpenses(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Expense).ToList();
    }

    public static decimal GetExpensesTotal(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Expense).Sum(x => x.Total);
    }

    public static List<Transaction> GetIncomes(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Income).ToList();
    }

    public static decimal GetIncomesTotal(this IEnumerable<Transaction> transactions)
    {
        return transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Income).Sum(x => x.Total);
    }

    public static decimal GetTotal(this IEnumerable<Transaction> transactions)
    {
        var expenses = transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Expense).Sum(x => x.Total);
        var incomes = transactions.Where(x => !x.IsDeleted && x.Type == TransactionType.Income).Sum(x => x.Total);
        var total = incomes - expenses;

        return total;
    }
}

