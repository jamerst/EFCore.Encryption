using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Encryption.OData;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddODataHashEquals(this IServiceCollection builder, params Type[] hashTypes)
    {
        HashedValueFilterBinder.HashTypes.AddRange(hashTypes.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHashedValue<>)));

        builder.AddSingleton<IFilterBinder, HashedValueFilterBinder>();
        return builder;
    }
}