using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests.Data;

public abstract class TestContextBase : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected string? StoreType { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (StoreType != null)
        {
            options
                .UseHashedType<HashedString, string>(StoreType)
                .UseHashedType<HashedCaseInsensitiveString, string>(StoreType)
                .UseHashedType<HashedDateOnly, DateOnly>(StoreType);
        }
        else
        {
            options
                .UseHashedType<HashedString, string>()
                .UseHashedType<HashedCaseInsensitiveString, string>()
                .UseHashedType<HashedDateOnly, DateOnly>();
        }

    }
}