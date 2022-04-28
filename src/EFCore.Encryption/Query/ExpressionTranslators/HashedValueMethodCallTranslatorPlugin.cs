using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.Encryption.Query.ExpressionTranslators;

internal class HashedValueMethodCallTranslatorPlugin<THash, TValue> : IMethodCallTranslatorPlugin
    where THash : IHashedValue<TValue>, new()
{
    public HashedValueMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
    {
        Translators = new IMethodCallTranslator[]
        {
            new HashedValueMethodCallTranslator<THash, TValue>(sqlExpressionFactory)
        };
    }

    public virtual IEnumerable<IMethodCallTranslator> Translators { get; }
}