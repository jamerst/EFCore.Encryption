using Microsoft.EntityFrameworkCore.Infrastructure;

using EFCore.Encryption.Infrastructure;

namespace EFCore.Encryption;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseHashedType<THash, TValue>(this DbContextOptionsBuilder optionsBuilder)
        where THash : IHashedValue<TValue>, new()
    {
        var extension = optionsBuilder.Options.FindExtension<HashedValueTypeOptionsExtension<THash, TValue>>() ?? new HashedValueTypeOptionsExtension<THash, TValue>();

        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}