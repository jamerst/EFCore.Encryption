using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.Encryption.Storage;

internal class HashedValueValueConverter<THash, TValue> : ValueConverter<THash, string>
    where THash : IHashedValue<TValue>, new()
{
    public HashedValueValueConverter() : base(i => toProvider(i), d => fromProvider(d)) { }

    private static string toProvider(THash hash)
    {
        return hash.Value;
    }

    private static THash fromProvider(string hash)
    {
        return new THash() { Value = hash };
    }
}
