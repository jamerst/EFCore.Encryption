namespace EFCore.Encryption.Tests.MySql;

#if RUNNING_IN_CONTAINER
public class MySqlTests : DbTestBase<MySqlTestFixture, MySqlTestContext>
{
    public MySqlTests(MySqlTestFixture fixture) : base(fixture) { }
}
#endif