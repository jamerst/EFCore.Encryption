using EFCore.Encryption.Tests.Data;

namespace EFCore.Encryption.Tests.Sqlite;

public class SqliteTestContext : TestContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={Path.Join(Environment.CurrentDirectory, "test.db")}");

        base.OnConfiguring(options);
    }
}