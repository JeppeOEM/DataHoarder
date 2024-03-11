using Microsoft.EntityFrameworkCore;
using TimescaleSample.Models;
using Dapper;
using System.Diagnostics;
using TimescaleSample.Data;
using Microsoft.Extensions.Configuration;




namespace TimescaleSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //returns config, looks for "ConnectionStrings"(dont change) in appsettings.json
            // IConfiguration config = new ConfigurationBuilder()
            //     .AddJsonFile("appSettings.json")
            //     .Build();

            var db = new StocksDbContext();

            // The use of @ before the string (@"...") allows you to write the SQL query as a multiline string without escaping each double quote (")

            var sql = @"
                        SELECT * FROM ""stocks_real_time""
                        WHERE ""symbol"" = 'MSFT'
                        ";

            var trades = db.Stocks.FromSqlRaw(sql).Count();
            Console.WriteLine($"{trades} trades of MSFT in the last week");


            Coin coin = new Coin()
            {
                Price = 100000,
                Time = DateTime.Now,
                Day_Volume = 100
            };


            // Specify a UTC DateTime for the weekly results query
            var date = new DateTime(2024, 03, 09, 0, 0, 0, DateTimeKind.Utc);

            // Retrieve the top result for a specific symbol (e.g., "MSFT")
            var topResult = db.GetWeeklyResults(date).FirstOrDefault(x => x.Symbol == "MSFT");

            if (topResult != null)
            {
                Console.WriteLine($"{topResult.Name} ({topResult.Symbol}): {topResult.Start:C} - {topResult.End:C} ~{topResult.Average:C}");
            }
            else
            {
                Console.WriteLine("No weekly results found for MSFT.");
            }
            var functionName = "get_weekly_results";
            var functionExists = db.Database.ExecuteSqlRaw(
                "SELECT COUNT(*) FROM information_schema.routines WHERE routine_name = {0}", functionName);

            if (functionExists > 0)
            {
                Console.WriteLine($"The database function '{functionName}' exists.");
            }
            else
            {
                Console.WriteLine($"The database function '{functionName}' does not exist.");
            }

            // DataContextDapper dapper = new DataContextDapper();


            // DateTime rightNow = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");


            // var tradeCount = db.Stocks.FromSqlRaw(sql).FirstOrDefault();
            // Console.WriteLine($"{tradeCount} trades of MSFT in total");

            // UTC Only
            // Read this and cry https://www.npgsql.org/doc/types/datetime.html
            // var date = new DateTime(2022, 06, 29, 0, 0, 0, DateTimeKind.Utc);
            // var top = db
            //     .GetWeeklyResults(date)
            //     .First(x => x.Symbol == "MSFT");

            // Console.WriteLine($"{top.Name} ({top.Symbol}): {top.Start:C} - {top.End:C} ~{top.Average:C}");

        }

    }
}