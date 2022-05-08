using EFCore.Encryption.Tests.Data;

namespace EFCore.Encryption.Tests.MySql;

public class MySqlTestContext : TestContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySQL("Host=mysql; Database=test_db; Username=root; Password=test");

        base.OnConfiguring(options);
    }
}