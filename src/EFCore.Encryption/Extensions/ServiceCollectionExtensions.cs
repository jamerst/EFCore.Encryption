using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

using EFCore.Encryption.Query.ExpressionTranslators;
using EFCore.Encryption.Storage;

namespace EFCore.Encryption;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHashedType<THash, TValue>(this IServiceCollection services)
        where THash : IHashedValue<TValue>, new()
    {
        return services.AddHashedType<THash, TValue>("TEXT");
    }

    public static IServiceCollection AddHashedType<THash, TValue>(this IServiceCollection services, string storeType)
        where THash : IHashedValue<TValue>, new()
    {
        new EntityFrameworkRelationalServicesBuilder(services)
            .TryAdd<IRelationalTypeMappingSourcePlugin, HashedValueTypeMappingSourcePlugin<THash, TValue>>(
                (_) => new HashedValueTypeMappingSourcePlugin<THash, TValue>(storeType)
            )
            .TryAdd<IMethodCallTranslatorPlugin, HashedValueMethodCallTranslatorPlugin<THash, TValue>>();

        return services;
    }
}