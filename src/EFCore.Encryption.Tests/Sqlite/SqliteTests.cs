namespace EFCore.Encryption.Tests.Sqlite;

public class SqliteTests : DbTestBase<SqliteTestFixture, SqliteTestContext>
{
    public SqliteTests(SqliteTestFixture fixture) : base(fixture) { }
}