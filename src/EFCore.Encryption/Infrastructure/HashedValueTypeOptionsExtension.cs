using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

using EFCore.Encryption.Query.ExpressionTranslators;

namespace EFCore.Encryption.Infrastructure;

internal class HashedValueTypeOptionsExtension<THash, TValue> : IDbContextOptionsExtension
    where THash : IHashedValue<TValue>, new()
{
    public DbContextOptionsExtensionInfo? _info;

    public void ApplyServices(IServiceCollection services)
    {
        services.AddHashedType<THash, TValue>();
    }

    public DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

    public void Validate(IDbContextOptions options)
    {
        var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
        if (internalServiceProvider != null)
        {
            using (var scope = internalServiceProvider.CreateScope())
            {
                // Instant
                if (scope.ServiceProvider.GetService<IEnumerable<IMethodCallTranslatorPlugin>>()
                        ?.Any(s => s is HashedValueMethodCallTranslatorPlugin<THash, TValue>) != true)
                {
                    throw new InvalidOperationException("UseHashedType requires AddHashedType to be called on the internal service provider used.");
                }
            }
        }
    }

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension) { }

        private new HashedValueTypeOptionsExtension<THash, TValue> Extension => (HashedValueTypeOptionsExtension<THash, TValue>)base.Extension;

        public override bool IsDatabaseProvider => false;

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) => debugInfo[nameof(DbContextOptionsBuilderExtensions.UseHashedType)] = "1";

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

        public override string LogFragment => "using EFCore.Encryption";
    }
}