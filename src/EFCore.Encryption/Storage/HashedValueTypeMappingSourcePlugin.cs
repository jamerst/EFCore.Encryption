using System.Data;

using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Encryption.Storage;

internal class HashedValueTypeMappingSourcePlugin<THash, TValue> : IRelationalTypeMappingSourcePlugin
    where THash : IHashedValue<TValue>, new()
{
    private readonly string _storeType;

    public HashedValueTypeMappingSourcePlugin(string storeType)
    {
        _storeType = storeType;
    }

    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType == typeof(THash))
        {
            return new HashedValueTypeMapping<THash, TValue>(_storeType, DbType.String);
        }

        return null;
    }
}