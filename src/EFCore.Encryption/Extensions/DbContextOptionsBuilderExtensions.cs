using Microsoft.EntityFrameworkCore.Infrastructure;

using EFCore.Encryption.Infrastructure;

namespace EFCore.Encryption;

/// <summary>
/// Extensions to allow hashed types to be registered in the DbContext
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Add support for the hashed type <typeparamref name="THash"/>.
    /// </summary>
    /// <typeparam name="THash">Hashed type</typeparam>
    /// <typeparam name="TValue">Type of value to be hashed</typeparam>
    /// <param name="optionsBuilder">DbContext options builder</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder UseHashedType<THash, TValue>(this DbContextOptionsBuilder optionsBuilder)
        where THash : IHashedValue<TValue>, new()
    {
        var extension = optionsBuilder.Options.FindExtension<HashedValueTypeOptionsExtension<THash, TValue>>() ?? new HashedValueTypeOptionsExtension<THash, TValue>();

        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    /// <summary>
    /// Add support for the hashed type <typeparamref name="THash"/>, using <paramref name="storeType"/> as the database column type.
    /// </summary>
    /// <typeparam name="THash">Hashed type</typeparam>
    /// <typeparam name="TValue">Type of value to be hashed</typeparam>
    /// <param name="optionsBuilder">DbContext options builder</param>
    /// <param name="storeType">SQL data type to use for the hashed column.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder UseHashedType<THash, TValue>(this DbContextOptionsBuilder optionsBuilder, string storeType)
        where THash : IHashedValue<TValue>, new()
    {
        var extension = optionsBuilder.Options.FindExtension<HashedValueTypeOptionsExtension<THash, TValue>>() ?? new HashedValueTypeOptionsExtension<THash, TValue>(storeType);

        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}