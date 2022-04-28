using Sample.Models;

namespace Sample.Data;

public class SampleContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    private string _dbPath { get; }

    public SampleContext()
    {
        _dbPath = Path.Join(Environment.CurrentDirectory, "example.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options
        .UseSqlite($"Data Source={_dbPath}")
        .UseHashedType<HashedString, string>()
        .UseHashedType<HashedCaseInsensitiveString, string>()
        .UseHashedType<HashedDateOnly, DateOnly>();
}