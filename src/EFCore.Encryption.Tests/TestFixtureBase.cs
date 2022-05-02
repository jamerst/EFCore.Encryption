using EFCore.Encryption.Tests.Data;

namespace EFCore.Encryption.Tests;

public abstract class TestFixtureBase<TContext> : IDisposable
    where TContext : TestContextBase, new()
{
    public TestFixtureBase()
    {
        DbContext = new TContext();

        DbContext.Database.EnsureCreated();

        SeedData.Seed(DbContext);
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }

    public TContext DbContext { get; set; }
}