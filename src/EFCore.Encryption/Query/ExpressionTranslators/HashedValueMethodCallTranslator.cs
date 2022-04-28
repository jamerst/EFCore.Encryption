using System.Reflection;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.Encryption.Query.ExpressionTranslators;

internal class HashedValueMethodCallTranslator<THash, TValue> : IMethodCallTranslator
    where THash : IHashedValue<TValue>, new()
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public HashedValueMethodCallTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (instance?.Type == typeof(THash) && method.Name == nameof(IHashedValue<object>.HashEquals))
        {
            if (arguments[0] is SqlConstantExpression sqlConstant && sqlConstant.Value is TValue value)
            {
                THash hash = new THash();
                hash.Value = hash.GetHashString(value);

                return _sqlExpressionFactory.Equal(instance!, _sqlExpressionFactory.Constant(hash));
            }
        }

        return null;
    }
}