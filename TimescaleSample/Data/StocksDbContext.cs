using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TimescaleSample.Data;

//inherits from DBContext
//DBContext is EF class for making connection to DB
public class StocksDbContext : DbContext
{

    //In Entity Framework, the DbSet<T> class is typically used to represent a table in a database.
    public DbSet<Stock> Stocks { get; set; } = default!;

    //default;
    // For reference types(classes, interfaces, delegates) the default value is null. 
    //For numeric types(such as int, float, etc.), 
    // the default value is 0. For bool, it's falsew, and for char, it's '\0'.

    public DbSet<Company> Companies { get; set; } = default!;

    public IQueryable<IntervalResult> GetWeeklyResults(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            // Read this and cry https://www.npgsql.org/doc/types/datetime.html
            throw new ArgumentException("DateTime.Kind must be of UTC to convert to timestamp with time zone");
        }

        return FromExpression(() => GetWeeklyResults(value));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseNpgsql(connectionString: "Server=localhost;User Id=postgres;Password=password;Database=postgres;")
        //.LogTo(Console.WriteLine)
        ;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // shouldn't be used since we have a method
        modelBuilder
            .HasDbFunction(typeof(StocksDbContext).GetMethod(nameof(GetWeeklyResults), new[] { typeof(DateTime) })!)
            // map to entity and don't worry about tables
            // mapping to a table in the snapshot 
            .HasName("get_weekly_results")
            .IsBuiltIn(false);
    }
}


// Never tracked for changes; therefore, you can never insert, update, or delete when using the DbContext.
// Only support a subset of navigation mapping capabilities.
// Can’t map between two keyless entities
// Can be mapped to a defining query that acts as a data source similar to DbSet.

//check out ADO.NET for data ingestion
// No PK

[Keyless]
public class Stock
{
    public DateTimeOffset Time { get; set; }

    [ForeignKey( /* navigation property */ nameof(Company))]
    public string Symbol { get; set; } = "";

    public decimal? Price { get; set; }
    public int? DayVolume { get; set; }
    public Company Company { get; set; } = default!;
}

public class Company
{
    [Key] public string Symbol { get; set; } = "";
    public string Name { get; set; } = "";
}

/// <summary>
/// Checkout the last migration to see the PostgreSQL function
/// 20220726184445_AddGetWeeklyResultsFunction.cs
/// <include file='20220726184445_AddGetWeeklyResultsFunction.cs' path='[@name="See ./TimescaleSample/Migrations/20220726184445_AddGetWeeklyResultsFunction.cs"]'/>
/// </summary>
/// <remarks></remarks>
[Keyless]
public record IntervalResult(
    string Symbol,
    string Name,
    decimal Start,
    decimal End,
    decimal Average);