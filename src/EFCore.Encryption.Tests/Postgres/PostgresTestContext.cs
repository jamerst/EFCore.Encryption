using EFCore.Encryption.Tests.Data;

namespace EFCore.Encryption.Tests.Postgres;

public class PostgresTestContext : TestContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=postgres; Database=test_db; Username=test; Password=test");

        base.OnConfiguring(options);
    }
}