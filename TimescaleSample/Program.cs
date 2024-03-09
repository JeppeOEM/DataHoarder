using Microsoft.EntityFrameworkCore;
using TimescaleSample.Models;

var db = new StocksDbContext();

// The use of @ before the string (@"...") allows you to write the SQL query as a multiline string without escaping each double quote (")

var sql = @"
SELECT * FROM ""stocks_real_time""
WHERE ""symbol"" = 'MSFT'
";

var trades = db.Stocks.FromSqlRaw(sql).Count();
Console.WriteLine($"{trades} trades of MSFT in the last week");


// var tradeCount = db.Stocks.FromSqlRaw(sql).FirstOrDefault();
// Console.WriteLine($"{tradeCount} trades of MSFT in total");

// UTC Only
// Read this and cry https://www.npgsql.org/doc/types/datetime.html
var date = new DateTime(2022, 06, 29, 0, 0, 0, DateTimeKind.Utc);
var top = db
    .GetWeeklyResults(date)
    .First(x => x.Symbol == "MSFT");

Console.WriteLine($"{top.Name} ({top.Symbol}): {top.Start:C} - {top.End:C} ~{top.Average:C}");