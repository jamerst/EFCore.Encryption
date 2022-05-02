using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests.Data;

public abstract class TestContextBase : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseHashedType<HashedString, string>()
            .UseHashedType<HashedCaseInsensitiveString, string>()
            .UseHashedType<HashedDateOnly, DateOnly>();
    }
}