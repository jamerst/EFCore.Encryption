using EFCore.Encryption.Tests.Data;

namespace EFCore.Encryption.Tests.SqlServer;

public class SqlServerTestContext : TestContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=mssql; Database=test_db; User Id=sa; Password=T3st!123");

        // The TEXT datatype in SQL Server is handled differently to other databases - it's not just an alias for NVARCHAR(MAX)
        // Need to override otherwise comparisons don't work, so might as well override as a fixed-length string
        StoreType = "NVARCHAR(44)";
        base.OnConfiguring(options);
    }
}