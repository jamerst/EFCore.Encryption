namespace EFCore.Encryption.Tests.SqlServer;

#if RUNNING_IN_CONTAINER
public class SqlServerTests : DbTestBase<SqlServerTestFixture, SqlServerTestContext>
{
    public SqlServerTests(SqlServerTestFixture fixture) : base(fixture) { }
}
#endif