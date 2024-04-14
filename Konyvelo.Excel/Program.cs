using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.Text;

namespace Konyvelo.Excel;

internal class Program
{
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        const string path = @"C:\Users\MARCI\Desktop\test.xlsx";

        using var file = File.Open(path, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(file, new ExcelReaderConfiguration
        {
            FallbackEncoding = Encoding.UTF8
        });
        using var data = reader.AsDataSet();
        using var table = data.Tables[0];

        var transfers = new List<Transaction>();
        foreach (var row in table.Rows)
        {
            if (row is not DataRow dataRow) continue;
            transfers.Add(new Transaction(dataRow));
        }

        //var begin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var begin = new DateTime(2024, 1, 1);
        var end = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
        var model = new PivotModel(begin, end, transfers);
        var content = CreateHtml(model);
        Console.Write(content);
        File.WriteAllText("out.txt", content, Encoding.UTF8);
    }

    static string CreateHtml(PivotModel pivotModel)
    {
        var sb = new StringBuilder();
        var numberFormatInfo = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ",",
            NumberGroupSizes = [3],
            NumberGroupSeparator = " "
        };

        sb.AppendLine($"*****{pivotModel.StartDate:yyyy.MM.dd}-{pivotModel.EndDate:yyyy.MM.dd}*****");
        foreach (var category in pivotModel.Categories.OrderBy(x => x.Category))
        {
            sb.AppendLine($"* {category.Category.ToUpper()}");
            foreach (var currency in category.Currencies.OrderBy(x => x.Currency).Select(x => new { x.Currency, x.Total }))
            {
                sb.AppendLine($"*  {currency.Currency.ToUpper()}: {currency.Total.ToString("N0", numberFormatInfo)}");
            }

            foreach (var currency in category.Currencies.OrderBy(x => x.Currency))
            {
                foreach (var transaction in currency.Transactions.OrderBy(x => x.Date))
                {
                    sb.AppendLine($"*    {transaction.Date:yyyy.MM.dd}: {transaction.Name}: {transaction.Total.ToString("N0", numberFormatInfo)} {transaction.Currency.ToUpper()}");
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}