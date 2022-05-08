namespace EFCore.Encryption.Tests.Postgres;

#if RUNNING_IN_CONTAINER
public class PostgresTests : DbTestBase<PostgresTestFixture, PostgresTestContext>
{
    public PostgresTests(PostgresTestFixture fixture) : base(fixture) { }
}
#endif