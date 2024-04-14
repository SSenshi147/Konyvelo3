using System.Data;

namespace Konyvelo.Excel;

public class Transaction
{
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Name { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string Currency { get; set; }

    public Transaction(DataRow dataRow)
    {
        var cols = dataRow.ItemArray;
        Date = DateTime.Parse(cols[0].ToString());
        Category = (string)cols[1];
        Name = cols[2] is null || cols[2] is DBNull ? string.Empty : (string?)cols[2];
        Total = decimal.Parse(cols[3].ToString());
        Currency = (string)cols[4];
    }
}