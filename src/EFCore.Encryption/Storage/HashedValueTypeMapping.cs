using System.Data;

using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Encryption.Storage;

internal class HashedValueTypeMapping<THash, TValue> : StringTypeMapping
    where THash : IHashedValue<TValue>, new()
{
    public HashedValueTypeMapping(string storeType, DbType? dbType, bool unicode = false, int? size = null)
        : base(CreateRelationalTypeMappingParameters(storeType, dbType, unicode, size)) { }

    private static RelationalTypeMappingParameters CreateRelationalTypeMappingParameters(string storeType, DbType? dbType, bool unicode, int? size)
    {
        return new RelationalTypeMappingParameters(
            new CoreTypeMappingParameters(
                typeof(THash),
                new HashedValueValueConverter<THash, TValue>()
            ),
            storeType,
            StoreTypePostfix.None,
            dbType,
            unicode,
            size
        );
    }

    protected HashedValueTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new HashedValueTypeMapping<THash, TValue>(parameters);
    }
}